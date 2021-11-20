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
            foreach (Question question in questions)
            {
                question.Answers = await _context.Answers.Where(a => a.QuestionId == question.Id).ToListAsync();
                questionsDTOList.Add(QuestionDTOResponseA.ConvertToResponse(question));
            }

            return Ok(questionsDTOList);
        }

        // GET: api/Question/5
        [HttpGet("questions/{id}")]
        public async Task<ActionResult<Question>> GetQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);

            if (question == null)
            {
                return NotFound();
            }

            return question;
        }

        // PUT: api/Question/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("questions/{id}")]
        public async Task<IActionResult> PutQuestion(int id, Question question)
        {
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
        public async Task<ActionResult<QuestionDTORequest>> PostQuestion(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuestion", new { id = question.Id }, question);
        }
        [Route("questions/answers")]
        [HttpPost]
        public async Task<ActionResult<Question>> PostQuestionWithAnswer(FullQuestion question)
        {
            _context.Questions.Add(question.Question);
            await _context.SaveChangesAsync();
            foreach (Answer answer in question.Answers)
            {
                answer.QuestionId = question.Question.Id;
                _context.Answers.Add(answer);
            }
            await _context.SaveChangesAsync();
            return Ok(question);
            //CreatedAtAction("GetQuestion", new { id = question.Id }, question);
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
    }
}
