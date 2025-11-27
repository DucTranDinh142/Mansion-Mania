public enum StatType
{
    Vitality,// mỗi điểm tương đương 5 máu tối đa và 1 giáp
    Strength,// mỗi điểm tương đương 1 điểm tấn công và 0.5% sát thương chí mạng
    Agility,// mỗi điểm tương đương 0,5% tỷ lệ né tránh và 0.3% tỷ lệ chí mạng
    Intelligence,//mỗi điểm tương đương 1 điểm sát thương nguyên tố và 0.5% chống chịu nguyên tố

    MaxHP,//Máu tối đa
    HealthRegen,//Hồi phục máu

    Damage,//Điểm tấn công
    AttackSpeed,//Tốc đánh
    CritPower,//Sát thương chí mạng
    CritChance,//Tỷ lệ chí mạng
    ArmorReduction,//Điểm xuyên giáp
    IceDamage,//Nguyên tố băng
    FireDamage,//Nguyên tố lửa
    LightningDamage,//Nguyên tố điện/năng lượng
    ElementalDamage,//Kết hợp sức mạnh của cả ba nguyên tố, với nguyên tố mạnh nhất được cộng đủ sát thương và gây hiệu ứng tương ứng, hai nguyên tố còn lại bổ trở 50% lượng điểm của chúng cho chỉ số này

    Armor,//Giáp
    EvasionChance,//Tỷ lệ né tránh
    FireResistance,//Kháng lửa
    IceResistance,//Kháng băng
    LightningResistance,//Kháng điện/năng lượng
}
