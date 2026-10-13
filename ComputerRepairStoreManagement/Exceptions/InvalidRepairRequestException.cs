using System;

namespace ComputerRepairStoreManagement.Exceptions
{
    public class InvalidRepairRequestException : Exception
    {
        public InvalidRepairRequestException() : base("The repair request data is invalid.")
        {
        }

        public InvalidRepairRequestException(string message) : base(message)
        {
        }

        public InvalidRepairRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
