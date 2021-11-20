using System.ComponentModel.DataAnnotations.Schema;

namespace project_cursed.Models
{


    public class Answer
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [System.Text.Json.Serialization.JsonIgnore]
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Correct { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Question? Question { get; set; }
        public int QuestionId { get; set; }
    }
    public class FullQuestion
    {
        public Question Question { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}