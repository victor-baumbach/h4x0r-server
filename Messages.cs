using FlatBuffers;

namespace h4x0r
{
    public class Messages
    {
        public static byte[] CreateAccountMessage(string username, string email, string password)
        {
            FlatBufferBuilder bb = new FlatBufferBuilder(2);

            var messageOffset = MessagesInternal.CreateAccountMessage.CreateCreateAccountMessage(bb, bb.CreateString(username), bb.CreateString(email), bb.CreateString(password));

            var baseOffset = MessagesInternal.MessageBase.CreateMessageBase(bb, MessagesInternal.MessageContainer.CreateAccountMessage, messageOffset.Value);
            bb.Finish(baseOffset.Value);

            var ms = new System.IO.MemoryStream(bb.DataBuffer.Data, bb.DataBuffer.Position, bb.Offset);
            return ms.ToArray();
        }

        // Must be kept in sync with the enum in MessagesInternal.fbs
        public enum CreateAccountResult
        {
            Success,
            AlreadyExists
        }

        public static byte[] CreateAccountResultMessage(CreateAccountResult result)
        {
            FlatBufferBuilder bb = new FlatBufferBuilder(2);

            var messageOffset = MessagesInternal.CreateAccountResultMessage.CreateCreateAccountResultMessage(bb, (MessagesInternal.CreateAccountResult)result);

            var baseOffset = MessagesInternal.MessageBase.CreateMessageBase(bb, MessagesInternal.MessageContainer.CreateAccountMessage, messageOffset.Value);
            bb.Finish(baseOffset.Value);

            var ms = new System.IO.MemoryStream(bb.DataBuffer.Data, bb.DataBuffer.Position, bb.Offset);
            return ms.ToArray();
        }
    }

}