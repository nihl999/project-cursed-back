using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_cursed.Models;

namespace project_cursed
{
    [Route("api/")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly SQLiteContext _context;

        public QuestionController(SQLiteContext context)
        {
            _context = context;
        }
        #region Questions
        [Route("questions")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDTOResponse>>> GetQuestions()
        {
            var questions = await _context.Questions.ToListAsync();

            return Ok(questions.Select(question => QuestionDTOResponse.ConvertToResponse(question)));
        }

        [Route("questions/answers")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDTOResponseA>>> GetQuestionsA()
        {

            var questions = await _context.Questions.ToListAsync();
            var questionsDTOList = new List<QuestionDTOResponseA>();
            if (questions == null)
                return NotFound();

            foreach (Question question in questions)
            {
                question.Answers = await _context.Answers.Where(a => a.QuestionId == question.Id).ToListAsync();
                questionsDTOList.Add(QuestionDTOResponseA.ConvertToResponse(question));
            }

            return Ok(questionsDTOList);
        }

        // GET: api/Question/5
        [HttpGet("questions/{id}")]
        public async Task<ActionResult<QuestionDTOResponse>> GetQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);

            if (question == null)
                return NotFound();


            return QuestionDTOResponse.ConvertToResponse(question);
        }

        // PUT: api/Question/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("questions/{id}")]
        public async Task<IActionResult> PutQuestion(int id, QuestionDTORequest questionRequest)
        {
            var question = QuestionDTORequest.ConvertFromRequest(questionRequest);

            if (id != question.Id)
            {
                return BadRequest();
            }

            _context.Entry(question).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Question
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("questions")]
        [HttpPost]
        public async Task<ActionResult<QuestionDTORequest>> PostQuestion(QuestionDTORequest questionRequest)
        {
            var question = QuestionDTORequest.ConvertFromRequest(questionRequest);
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuestion", new { id = question.Id }, question);
        }

        [Route("questions/answers")]
        [HttpPost]
        public async Task<ActionResult<QuestionDTOResponse>> PostQuestionWithAnswer(QuestionDTORequestA questionRequest)
        {
            var question = QuestionDTORequestA.ConvertFromRequest(questionRequest);
            _context.Questions.Add(question);
            foreach (Answer answer in question.Answers)
            {
                answer.Question = question;
            }
            _context.Answers.AddRange(question.Answers);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetQuestion", new { id = question.Id }, question);
        }

        [HttpDelete("questions/{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }
        #endregion
        #region Answers
        [Route("answers")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnswerDTOResponse>>> GetAnswers()
        {
            var answers = await _context.Answers.ToListAsync();

            return Ok(answers.Select(answer => AnswerDTOResponse.ConvertToResponse(answer)));
        }

        [HttpGet("answers/{id}")]
        public async Task<ActionResult<AnswerDTOResponse>> GetAnswer(int id)
        {
            var answer = await _context.Answers.FindAsync(id);

            if (answer == null)
                return NotFound();


            return AnswerDTOResponse.ConvertToResponse(answer);
        }

        [Route("answers")]
        [HttpPost]
        public async Task<ActionResult<AnswerDTOResponse>> PostAnswer(AnswerDTORequest answerRequest)
        {
            var answer = AnswerDTORequest.ConvertFromRequest(answerRequest);
            if (!QuestionExists(answer.QuestionId))
                return NotFound($"Question ID {answer.QuestionId} doesn't exist");
            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnswer", new { id = answer.Id }, answer);
        }
        [HttpPut("answers/{id}")]
        public async Task<IActionResult> PutAnswer(int id, AnswerDTORequest answerRequest)
        {
            var answer = AnswerDTORequest.ConvertFromRequest(answerRequest);

            _context.Entry(answer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("answers/{id}")]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            var answer = await _context.Answers.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }

            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool AnswerExists(int id)
        {
            return _context.Answers.Any(e => e.Id == id);
        }
        #endregion
    }
}
