using Yitter.IdGenerator;

namespace ZhonTai.Api.Core.IdGenerator;

public class IdHelper
{
    private static bool _isSet = false;
    private static readonly object _locker = new();

    public static byte WorkerIdBitLength => 6;
    public static byte SeqBitLength => 6;
    public static short MaxWorkerId => (short)(Math.Pow(2.0, WorkerIdBitLength) - 1);
    public static short CurrentWorkerId { get; private set; } = -1;

    /// <summary>
    /// 初始化Id生成器
    /// </summary>
    /// <param name="workerId"></param>
    public static void SetIdGenerator(ushort workerId)
    {
        if (_isSet)
            throw new InvalidOperationException("只允许一次");

        if (workerId > MaxWorkerId || workerId < 0)
            throw new ArgumentException($"WorkerId 不能大于 {MaxWorkerId} 或小于 0");

        lock (_locker)
        {
            if (_isSet)
                throw new InvalidOperationException("只允许一次");

            YitIdHelper.SetIdGenerator(new IdGeneratorOptions(workerId)
            {
                WorkerIdBitLength = WorkerIdBitLength,
                SeqBitLength = SeqBitLength
            });

            CurrentWorkerId = (short)workerId;
            _isSet = true;
        }
    }

    /// <summary>
    /// 获取唯一Id
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static long GetNextId()
    {
        if (!_isSet)
            throw new InvalidOperationException("请先调用SetIdGenerator");

        return YitIdHelper.NextId();
    }
}
