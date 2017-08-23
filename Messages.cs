using FlatBuffers;
using System;

namespace h4x0r
{
    public class Messages
    {
        private static byte[] PrefixMessageLength(FlatBufferBuilder bb)
        {
            byte[] message = bb.SizedByteArray();
            byte[] prefix = BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)message.Length));
            byte[] prefixedMessage = new byte[prefix.Length + message.Length];
            Buffer.BlockCopy(prefixedMessage, 0, prefixedMessage, 0, prefix.Length);
            Buffer.BlockCopy(message, 0, prefixedMessage, prefix.Length, message.Length);
            return prefixedMessage;
        }

        public static short GetMessageLength(byte[] buffer)
        {
            if (buffer.Length < 2)
            {
                return 0;
            }
            else
            {
                short len = System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, 0));
                return (len > 0) ? len : (short)0;
            }
        }
        
        public static byte[] CreateAccountMessage(string username, string email, string password)
        {
            FlatBufferBuilder bb = new FlatBufferBuilder(2);

            var messageOffset = MessagesInternal.CreateAccountMessage.CreateCreateAccountMessage(bb, bb.CreateString(username), bb.CreateString(email), bb.CreateString(password));

            var baseOffset = MessagesInternal.MessageBase.CreateMessageBase(bb, MessagesInternal.MessageContainer.CreateAccountMessage, messageOffset.Value);
            bb.Finish(baseOffset.Value);

            return PrefixMessageLength(bb);
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

            var baseOffset = MessagesInternal.MessageBase.CreateMessageBase(bb, MessagesInternal.MessageContainer.CreateAccountResultMessage, messageOffset.Value);
            bb.Finish(baseOffset.Value);

            return PrefixMessageLength(bb);
        }

        public static byte[] LoginMessage(string username, string password)
        {
            FlatBufferBuilder bb = new FlatBufferBuilder(2);

            var messageOffset = MessagesInternal.LoginMessage.CreateLoginMessage(bb, bb.CreateString(username), bb.CreateString(password));

            var baseOffset = MessagesInternal.MessageBase.CreateMessageBase(bb, MessagesInternal.MessageContainer.LoginMessage, messageOffset.Value);
            bb.Finish(baseOffset.Value);

            return PrefixMessageLength(bb);
        }

        // Must be kept in sync with the enum in MessagesInternal.fbs
        public enum LoginResult
        {
            Success,
            Failed,
            Banned
        }

        public static byte[] LoginResultMessage(LoginResult result)
        {
            FlatBufferBuilder bb = new FlatBufferBuilder(2);

            var messageOffset = MessagesInternal.LoginResultMessage.CreateLoginResultMessage(bb, (MessagesInternal.LoginResult)result);

            var baseOffset = MessagesInternal.MessageBase.CreateMessageBase(bb, MessagesInternal.MessageContainer.LoginResultMessage, messageOffset.Value);
            bb.Finish(baseOffset.Value);

            return PrefixMessageLength(bb);
        }

        public static byte[] UpdateAddressMessage(string address, string hostname)
        {
            FlatBufferBuilder bb = new FlatBufferBuilder(2);

            var messageOffset = MessagesInternal.UpdateAddressMessage.CreateUpdateAddressMessage(bb,
                bb.CreateString(address),
                hostname.Length == 0 ? default(StringOffset) : bb.CreateString(hostname));

            var baseOffset = MessagesInternal.MessageBase.CreateMessageBase(bb, MessagesInternal.MessageContainer.UpdateAddressMessage, messageOffset.Value);
            bb.Finish(baseOffset.Value);

            return PrefixMessageLength(bb);
        }

        public static byte[] UpdateCreditsMessage(long credits)
        {
            FlatBufferBuilder bb = new FlatBufferBuilder(2);

            var messageOffset = MessagesInternal.UpdateCreditsMessage.CreateUpdateCreditsMessage(bb, credits);

            var baseOffset = MessagesInternal.MessageBase.CreateMessageBase(bb, MessagesInternal.MessageContainer.UpdateCreditsMessage, messageOffset.Value);
            bb.Finish(baseOffset.Value);

            return PrefixMessageLength(bb);
        }

        public static byte[] UpdateReputationMessage(int reputation)
        {
            FlatBufferBuilder bb = new FlatBufferBuilder(2);

            var messageOffset = MessagesInternal.UpdateReputationMessage.CreateUpdateReputationMessage(bb, reputation);

            var baseOffset = MessagesInternal.MessageBase.CreateMessageBase(bb, MessagesInternal.MessageContainer.UpdateReputationMessage, messageOffset.Value);
            bb.Finish(baseOffset.Value);

            return PrefixMessageLength(bb);
        }
    }

}