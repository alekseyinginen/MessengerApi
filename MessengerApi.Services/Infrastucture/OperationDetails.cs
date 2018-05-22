namespace MessengerApi.BLL.Infrastucture
{    public class OperationDetails
    {
        public OperationDetails(bool succedeed, string message, string property) {
            Succedeed = succedeed;
            Message = message;
            Property = property;
        }

        public bool Succedeed { get; private set; }

        public string Message { get; private set; }

        public string Property { get; private set; }
    }
}
