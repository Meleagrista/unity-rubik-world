using System.Collections.Generic;

public class MoveLog
{
    private readonly List<MoveRecord> m_records = new List<MoveRecord>();

    public IReadOnlyList<MoveRecord> Records => m_records;

    public void Record(MoveRecord record)
    {
        m_records.Add(record);
    }

    public bool TryPop(out MoveRecord record)
    {
        if (m_records.Count == 0)
        {
            record = default;
            return false;
        }

        record = m_records[^1];
        m_records.RemoveAt(m_records.Count - 1);
        return true;
    }

    public void Clear()
    {
        m_records.Clear();
    }
}
