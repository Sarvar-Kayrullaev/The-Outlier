namespace Interfaces
{
    public enum CharacterFactions
    {
        Player,
        Allies,
        Civilians,
        Nomads,
        Raiders,
        Zombies,
        Animals
    }
    
    public enum CharacterState
    {
        Idle,
        Guarding,
        Wander,      // Random rotation (for animals and Civilians)
        Suspicious,  // (if it hears a sound)
        Alert,       // (if the suspicion is confirmed)
        Chasing,
        Attacking,
        Fleeing,
        Patrol,
        Searching,   // (if lost)
        Dead
    }
    
    public enum AIObjective
    {
        None,           // Hech qanday maxsus topshiriq yo'q
        Sentry,         // Minora yoki balandlikda turib kuzatish (Snayperlar)
        PatrolPerimeter,// Outpost tashqarisi yoki ichida aylanish
        Worker,         // Qurol tuzatish, yuk tashish (O'yinchi ko'rmaguncha chalg'igan holat)
        AlarmOperator,  // Xavf tug'ilganda signalizatsiyaga yugurish (Eng muhim rol)
        Reinforcement,  // Faqat signal chalingandan keyin paydo bo'ladigan dushmanlar
        GuardArea,      // Hududni himoya qilish
        HoldHostage,    // Garovda ushlash
        DefendTarget,   // Biror obyekt yoki shaxsni himoya qilish
        PatrolPath,     // Ma'lum bir yo'nalish bo'ylab yurish
        HideAndSeek     // Yashirinib hujum qilish
    }
    
    public enum AITacticalRole
    {
        Soldier,    // Oddiy hujumchi
        Sniper,     // Uzoqdan otuvchi
        Heavy,      // Sekin harakatlanuvchi, kuchli bronyali
        Flanker,    // O'yinchining yon tomondan aylanib o'tishga harakat qiladigan (Shooter)
        Supporter   // Signalizatsiyani yoquvchi yoki granata uloqtiruvchi
    }
    
    public enum ItemCategory
    {
        Weapon,
        Consumable,
        Resource,
        Ammo,
        Clothing,
        QuestItem
    }
    
    public enum MissionStatus
    {
        NotStarted,
        Active,
        Completed,
        Failed
    }
    public enum AbilityNodeDirection
    {
        Center,
        Top,
        Bottom,
        Left,
        Right
    }
}