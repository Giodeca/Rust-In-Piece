public interface ITriggerCeckable
{
    bool isAggroed { get; set; }
    bool isInSight { get; set; }
    bool isInArea { get; set; }

    void SetAggroStatus(bool isAggroed);
    void SetStrikingBool(bool isInSight);

    void SetPatrolActive(bool isInArea);
}
