using FlatBuffers;
using System;
using System.Collections.Generic;

namespace h4x0r
{
    public class NetworkMessageException : Exception
    {
        public NetworkMessageException()
        {
        }

        public NetworkMessageException(string message)
            : base(message)
        {
        }

        public NetworkMessageException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class NetworkMessage
    {
        public NetworkMessage(byte[] buffer, int bytesToProcess)
        {
            m_Messages = new List<byte[]>();

            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(buffer, 0, bytesToProcess);
            while (bytesToProcess > 0)
            {
                byte[] lengthBuffer = new byte[2];
                if (memoryStream.Read(lengthBuffer, 0, 2) != 2)
                {
                    throw new NetworkMessageException("Invalid length field");
                }
                bytesToProcess -= 2;

                short messageLength = System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt16(lengthBuffer, 0));
                if (messageLength <= 0)
                {
                    throw new NetworkMessageException("Length field <= 0");
                }

                byte[] message = new byte[messageLength];
                if (memoryStream.Read(message, 0, messageLength) != messageLength)
                {
                    throw new NetworkMessageException("Message length mismatch");
                }
                m_Messages.Add(message);
                bytesToProcess -= messageLength;
            }
        }

        public IEnumerator<byte[]> GetEnumerator()
        {
            return m_Messages.GetEnumerator();
        }

        List<byte[]> m_Messages;
    }

    public class Messages
    {
        private static byte[] PrefixMessageLength(FlatBufferBuilder bb)
        {
            byte[] message = bb.SizedByteArray();
            byte[] prefix = BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)message.Length));
            byte[] prefixedMessage = new byte[prefix.Length + message.Length];
            Buffer.BlockCopy(prefix, 0, prefixedMessage, 0, prefix.Length);
            Buffer.BlockCopy(message, 0, prefixedMessage, prefix.Length, message.Length);
            return prefixedMessage;
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

        public static byte[] UpdateKnownAddressMessage(string address, string hostname, Node.Type type)
        {
            FlatBufferBuilder bb = new FlatBufferBuilder(2);

            var messageOffset = MessagesInternal.UpdateKnownAddressMessage.CreateUpdateKnownAddressMessage(bb,
                bb.CreateString(address),
                hostname.Length == 0 ? default(StringOffset) : bb.CreateString(hostname),
                (MessagesInternal.NodeType)type);

            var baseOffset = MessagesInternal.MessageBase.CreateMessageBase(bb, MessagesInternal.MessageContainer.UpdateKnownAddressMessage, messageOffset.Value);
            bb.Finish(baseOffset.Value);

            return PrefixMessageLength(bb);
        }

        public static byte[] NodeConnectMessage(string[] route)
        {
            FlatBufferBuilder bb = new FlatBufferBuilder(2);

            StringOffset[] offsets = new StringOffset[route.Length];
            for (int i = 0; i < route.Length; ++i)
            {
                offsets[i] = bb.CreateString(route[i]);
            }

            var messageOffset = MessagesInternal.NodeConnectMessage.CreateRouteVector(bb, offsets);

            var baseOffset = MessagesInternal.MessageBase.CreateMessageBase(bb, MessagesInternal.MessageContainer.NodeConnectMessage, messageOffset.Value);
            bb.Finish(baseOffset.Value);

            return PrefixMessageLength(bb);
        }

        // Must be kept in sync with the enum in MessagesInternal.fbs
        public enum NodeConnectResult
        {
            Success,
            Timeout,
            ConnectionRejected
        }

        public static byte[] NodeConnectResultMessage(NodeConnectResult success, Node.Type type)
        {
            FlatBufferBuilder bb = new FlatBufferBuilder(2);

            var messageOffset = MessagesInternal.NodeConnectResultMessage.CreateNodeConnectResultMessage(
                bb,
                (MessagesInternal.NodeConnectResult)success,
                (MessagesInternal.NodeType)type);

            var baseOffset = MessagesInternal.MessageBase.CreateMessageBase(bb, MessagesInternal.MessageContainer.NodeConnectResultMessage, messageOffset.Value);
            bb.Finish(baseOffset.Value);

            return PrefixMessageLength(bb);
        }
    }

}