using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;


namespace project_cursed.Models
{
    public class Answer
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Correct { get; set; }

        public virtual Question? Question { get; set; }
        public int QuestionId { get; set; }
    }
    [DisplayName("Answer.Response")]
    public class AnswerDTOResponse
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Correct { get; set; }
        public int QuestionId { get; set; }

        static public AnswerDTOResponse ConvertToResponse(Answer source)
        {
            return new AnswerDTOResponse
            {
                Id = source.Id,
                Text = source.Text,
                Correct = source.Correct,
                QuestionId = source.QuestionId
            };
        }
    }
    [DisplayName("Answer.Post")]
    public class AnswerDTORequest
    {
        public string Text { get; set; }
        public bool Correct { get; set; }
        public int QuestionId { get; set; }

        static public Answer ConvertFromRequest(AnswerDTORequest source)
        {
            return new Answer
            {
                Text = source.Text,
                Correct = source.Correct,
                QuestionId = source.QuestionId
            };
        }
    }
}