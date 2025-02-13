namespace Library.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(Type type, object key)
            : base($"Entity {type.Name} ({key}) was not found.")
        {

        }
        public NotFoundException(Type type)
            : base($"No {type.Name} entities were found.")
        {

        }
    }
}
