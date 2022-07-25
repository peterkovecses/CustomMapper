namespace CustomMapper.Interfaces
{
    public interface ICustomMapper
    {
        T2 Map<T1, T2>(T1 source) where T1 : class where T2 : class, new();
    }
}
