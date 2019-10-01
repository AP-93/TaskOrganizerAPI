using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskOrganizerAPI.Data;
using TaskOrganizerAPI.Dtos;
using TaskOrganizerAPI.Model;

namespace TaskOrganizerAPI.Controllers
{
   [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private DataContext _dataContext;
        public ValuesController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        //GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /*GET api/values/5
        [HttpGet("{id}")]
        public  ActionResult<string> Get(int id)
        {
            var userBoards = _dataContext.UserBoards.Where(x => x.UserId == id).ToList();

            List<Board> boards = new List<Board>();
            foreach(var b in userBoards)
            {
                boards.Add(b.Board);
            }
            return boards.ToString();
        }*/

        // GET 
        [HttpGet("getBoards/{id}")]
        public ActionResult<string> GetBoards(int id)
        {
            var userBoards = _dataContext.Users.Where(x => x.Id == id).SelectMany(x => x.Boards).Select(x => x.Board).Include(l => l.BoardLists).ThenInclude(c => c.Cards).ToList();//.OrderBy(crd=>crd.CardPosition)
            foreach (var b in userBoards)
            {
                b.BoardLists = b.BoardLists.OrderBy(x => x.ListPosition).ToList();
            }
            return Ok(userBoards);
        }

        [HttpGet("getLists/{id}")]
        public ActionResult<string> GetLists(int id)
        {
            var boardLists = _dataContext.Boards.Include(l => l.BoardLists).ThenInclude(c => c.Cards).FirstOrDefault(x => x.Id == id);
            boardLists.BoardLists.OrderBy(x => x.ListPosition);
            return Ok(boardLists);
        }
        [HttpGet("getCards/{id}")]
        public ActionResult<string> GetCards(int id)
        {
            var cards = _dataContext.Boards.Where(x => x.Id == id).SelectMany(x => x.BoardLists).SelectMany(x => x.Cards).ToList();
            cards.OrderBy(c => c.CardPosition);
            return Ok(cards);
        }

        [HttpGet("getBoardUsers/{id}")]
        public ActionResult<string> GetBoardUsers(int id)
        {
            int ownerID = _dataContext.Boards.FirstOrDefault(x => x.Id == id).OwnerId;
            var userNameAndId = _dataContext.Boards.Where(x => x.Id == id ).SelectMany(x => x.Users).Select(u => u.User).Select(d => new UserIdAndName {
                Id= d.Id,
                Username= d.Username
            }).ToList();

            var index = userNameAndId.FindIndex(x => x.Id == ownerID);
            var item = userNameAndId[index];
            userNameAndId[index] = userNameAndId[0];
            userNameAndId[0] = item;

            return Ok(userNameAndId);
        }

       //PUT
        [HttpPut("addUserToBoard/{id}")]
        public async Task<IActionResult> AddUserToBoard(int id, [FromBody] UserIdAndName value)
        {
            var board = await _dataContext.Boards.Include(x=>x.Users).FirstOrDefaultAsync(x => x.Id == id);
            var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Username== value.Username);
            var existingUsers = board.Users.Select(u => u.UserId);
            if (user != null)
            {
                foreach(var i in existingUsers)
                {
                    if(i == user.Id)
                      return NotFound("User already added");
                }

                var userBoard = new UserBoard
                {
                    Board = board,
                    User = user
                };
                board.Users.Add(userBoard);
                _dataContext.SaveChanges();
                return Ok();
            }else
            return NotFound("User doesent exist");
        }

          // PUT api/values/5
        [HttpPut("moveList")]
        public async Task Put( [FromBody] ListToMove ltm)
        {
            var userBoards =  _dataContext.Users.Where(x => x.Id == ltm.userId).SelectMany(x => x.Boards).Select(x => x.Board).Include(l => l.BoardLists).ThenInclude(c => c.Cards).ToList();
            var currentBoard = userBoards.FirstOrDefault(x => x.Id == ltm.currentBoardId);
            var lists = currentBoard.BoardLists.OrderBy(pos=>pos.ListPosition).ToList();
            var selectedList = currentBoard.BoardLists.FirstOrDefault(x => x.Id == ltm.currentListId);

            if(ltm.currentBoardId != ltm.moveToBoardId)
            {
               var moveToBoard = userBoards.FirstOrDefault(x => x.Id == ltm.moveToBoardId);
                moveToBoard.BoardLists.Add(selectedList);
                currentBoard.BoardLists.Remove(selectedList);

                foreach (var a in currentBoard.BoardLists.Where(x => x.ListPosition > selectedList.ListPosition))
                {
                    a.ListPosition--;
                }
                foreach (var a in moveToBoard.BoardLists.Where(x => x.ListPosition >= ltm.moveToListId))
                {
                    a.ListPosition++;
                }
            }
            else { 
               if(selectedList.ListPosition < ltm.moveToListId)
                {
                    foreach (var a in currentBoard.BoardLists.Where(x => x.ListPosition <= ltm.moveToListId && x.ListPosition > selectedList.ListPosition))
                    { a.ListPosition--; }
                }
               else if(selectedList.ListPosition > ltm.moveToListId)
                {
                    foreach (var a in currentBoard.BoardLists.Where(x => x.ListPosition >= ltm.moveToListId && x.ListPosition < selectedList.ListPosition))
                    { a.ListPosition++; }
                }
            }
            selectedList.ListPosition = ltm.moveToListId;
            _dataContext.SaveChanges();
        }

        [HttpPut("ChangeCard/{boardId}")]
        public async Task ChangeCard(int boardId, [FromBody] CardForEdit card)
        {
            var b = _dataContext.Boards.Where(x => x.Id == boardId).SelectMany(l => l.BoardLists).Include(c=>c.Cards);
            var lst = b.FirstOrDefault(x => x.Id == card.BoardListId);
            var crd = lst.Cards.FirstOrDefault(c => c.Id == card.Id);

            crd.CardText = card.CardText;
            
            _dataContext.SaveChanges();
        }

        [HttpPut("MoveCard")]
        public async Task MoveCard( [FromBody] CardToMove ctm)
        {
            var b = await _dataContext.Boards.Include(l => l.BoardLists).ThenInclude(c => c.Cards).FirstOrDefaultAsync(brd=>brd.Id==ctm.currentBoardId);
            var currentList =  b.BoardLists.FirstOrDefault(x => x.Id == ctm.currentListId);
            var selectedCard = currentList.Cards.FirstOrDefault(c => c.Id == ctm.cardId);

            if (ctm.currentListId != ctm.moveToListId)
            {
                var moveToList = b.BoardLists.FirstOrDefault(x => x.Id == ctm.moveToListId);

                moveToList.Cards.Add(selectedCard);
                currentList.Cards.Remove(selectedCard);
               
                 foreach (var c in currentList.Cards.Where(x => x.CardPosition > selectedCard.CardPosition))
                {
                    c.CardPosition--;
                }
                foreach (var c in moveToList.Cards.Where(x => x.CardPosition >= ctm.moveToPosition))
                {
                    c.CardPosition++;
                }
            }

            else
            {
                if (selectedCard.CardPosition < ctm.moveToPosition)
                {
                    foreach (var a in currentList.Cards.Where(x => x.CardPosition <= ctm.moveToPosition && x.CardPosition > selectedCard.CardPosition))
                    { a.CardPosition--; }
                }
                else if (selectedCard.CardPosition > ctm.moveToPosition)
                {
                    foreach (var a in currentList.Cards.Where(x => x.CardPosition >= ctm.moveToPosition && x.CardPosition < selectedCard.CardPosition))
                    { a.CardPosition++; }
                }
            }
            selectedCard.CardPosition = ctm.moveToPosition;
            _dataContext.SaveChanges();
        }


        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPost("createBoard")]
        public async Task CreateBoard( [FromBody] BoardForCreate bfc)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == bfc.userId);
            var board = new Board
            {
                Title = bfc.BoardName,
                OwnerId = bfc.userId
            };
            board.Users = new List<UserBoard>
            {
                new UserBoard
                {
                    Board = board,
                    User = user,  
                }
            };
            _dataContext.Boards.Add(board);
            _dataContext.SaveChanges();
        }

        [HttpPost("createBoardList")]
        public async Task CreateBoardList([FromBody] ListForCreate lfc)
        {
            var board = await _dataContext.Boards.Include(x=>x.BoardLists).FirstOrDefaultAsync(x => x.Id == lfc.boardId);
            int listPositon=0;
             foreach(var l in board.BoardLists)
            {
                if (listPositon <= l.ListPosition)
                    listPositon = l.ListPosition;
            }
            if (board.BoardLists == null)
            {
                board.BoardLists = new List<BoardList>();
            }
            var boardList = new BoardList
            { ListPosition = listPositon + 1,
               ListName = lfc.ListName,
            };
            if(board.BoardLists.Count == 0)
            {
                boardList.ListPosition = 1; }
            else{
            boardList.ListPosition = board.BoardLists.OrderByDescending(p => p.ListPosition).FirstOrDefault().ListPosition + 1;
            }

            board.BoardLists.Add(boardList);
            _dataContext.SaveChanges();
        }

        [HttpPost("createCard")]
        public async Task CreateCard([FromBody] CardForCreate cfc)
        {
            var board = await _dataContext.Boards.Include(x => x.BoardLists).ThenInclude(c=>c.Cards).FirstOrDefaultAsync(x => x.Id == cfc.boardId);
            var list =  board.BoardLists.FirstOrDefault(x => x.Id == cfc.listId);
            int cardPosition = 0;

            if (list.Cards == null)
            {
                list.Cards = new List<Card>();
            }
         
                 foreach (var c in list.Cards)
                 {
                     if (cardPosition <= c.CardPosition)
                     cardPosition= c.CardPosition;
                 }
        

            var card = new Card
            {
                CardPosition = cardPosition + 1,
                CardText = cfc.cardName
            };
         

            list.Cards.Add(card);
            _dataContext.SaveChanges();
        }
        // DELETE api/values/5
        [HttpDelete("deleteBoard/{id}")]
        public async Task DeleteBoard(int id)
        {
            var board = await _dataContext.Boards.FirstOrDefaultAsync(x => x.Id == id);
            _dataContext.Remove(board);
            _dataContext.SaveChanges();
        }
        [HttpDelete("deleteList/{id}/{listId}")]
        public async Task DeleteList(int id, int listId)
        {
            var board = await _dataContext.Boards.Include(x => x.BoardLists).FirstOrDefaultAsync(x => x.Id == id);
            var list = board.BoardLists.FirstOrDefault(x => x.Id == listId);
            _dataContext.Remove(list);
            _dataContext.SaveChanges();
        }
        
        [HttpDelete("deleteCard/{id}/{listId}/{cardId}")]
        public async Task DeleteCard(int id, int listId, int cardId)
        {
            var board =await _dataContext.Boards.Include(x => x.BoardLists).ThenInclude(c => c.Cards).FirstOrDefaultAsync(i=>i.Id==id);
            var card =  board.BoardLists.FirstOrDefault(x => x.Id == listId).Cards.FirstOrDefault(x => x.Id == cardId);
              
            _dataContext.Remove(card);
            _dataContext.SaveChanges();
        }
        [HttpDelete("deleteAllCards/{id}/{listId}")]
        public async Task DeleteAllCards(int id, int listId)
        {
            var board = await _dataContext.Boards.Include(x => x.BoardLists).ThenInclude(c => c.Cards).FirstOrDefaultAsync(i => i.Id == id);
            var cards = board.BoardLists.FirstOrDefault(x => x.Id == listId).Cards;
            cards.Clear();
            
            _dataContext.SaveChanges();
        }
        [HttpDelete("removeUserFromBoard/{id}/{userId}")]
        public async Task removeUserFromBoard(int id, int userId)
        {
            var board = await _dataContext.Boards.Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == id);
            var userBoard = board.Users.FirstOrDefault(x => x.UserId== userId);

            _dataContext.UserBoards.Remove(userBoard);

            _dataContext.SaveChanges();
        }
    }
}
