namespace Zeitgeist.Web.Models.Page
{
    public class Notification
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int NotifyType { get; set; }
        public string UserName { get; set; }
        public bool IsReaded { get; set; }
    }

    public enum NotifyType
    {
        Avoid = 0,
        Alert = 1,
        Message = 2,
        Another = 3
    }
}