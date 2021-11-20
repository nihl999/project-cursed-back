using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;


namespace project_cursed.Models
{
    public class Question
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public string Subject { get; set; }
        public virtual ICollection<Answer>? Answers { get; set; }

    }
    [DisplayName("Question.ResponseWA")]
    public class QuestionDTOResponseA
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public string Subject { get; set; }
        public virtual ICollection<AnswerDTOResponse>? Answers { get; set; }

        static public QuestionDTOResponseA ConvertToResponse(Question source)
        {
            return new QuestionDTOResponseA()
            {
                Id = source.Id,
                Text = source.Text,
                Author = source.Author,
                Subject = source.Subject,
                Answers = source.Answers.Select(a => AnswerDTOResponse.ConvertToResponse(a)).ToList()
            };

        }
    }
    [DisplayName("Question.ResponseWOA")]
    public class QuestionDTOResponse
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public string Subject { get; set; }

        static public QuestionDTOResponse ConvertToResponse(Question source)
        {
            return new QuestionDTOResponse()
            {
                Id = source.Id,
                Text = source.Text,
                Author = source.Author,
                Subject = source.Subject
            };

        }
    }
    [DisplayName("Question.PostWA")]
    public class QuestionDTORequestA
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public int Id { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public string Subject { get; set; }
        public virtual ICollection<AnswerDTORequest>? Answers { get; set; }

        static public Question ConvertFromRequest(QuestionDTORequestA source)
        {
            Question temp = new Question()
            {
                Id = source.Id,
                Text = source.Text,
                Author = source.Author,
                Subject = source.Subject,
                Answers = source.Answers.Select(a => AnswerDTORequest.ConvertFromRequest(a)).ToList()
            };
            return temp;
        }
    }
    [DisplayName("Question.PostWOA")]
    public class QuestionDTORequest
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public int Id { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public string Subject { get; set; }

        static public Question ConvertFromRequest(QuestionDTORequest source)
        {
            Question temp = new Question()
            {
                Id = source.Id,
                Text = source.Text,
                Author = source.Author,
                Subject = source.Subject,
            };
            return temp;
        }
    }
}