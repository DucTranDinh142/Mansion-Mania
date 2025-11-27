using TMPro;
using UnityEngine;

public class UI_StatToolTip : UI_ToolTip
{
    private Player_Stats playerStats;
    private TextMeshProUGUI statTooltipText;

    protected override void Awake()
    {
        base.Awake();
        playerStats = FindFirstObjectByType<Player_Stats>();
        statTooltipText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ShowToolTip(bool show, RectTransform targetRect, StatType statType)
    {
        base.ShowToolTip(show, targetRect);
        statTooltipText.text = GetStatTextByType(statType);
    }
    public string GetStatTextByType(StatType statType)
    {
        switch (statType)
        {
            case StatType.MaxHP: return "Chỉ số quy định lượng máu tối đa có thể đạt được.\nNếu hết máu sẽ tử trận.";
            case StatType.HealthRegen: return "Chỉ số quy định lượng máu hồi phục tự nhiên theo thời gian mỗi một giây.";
            case StatType.Strength: return "Với mỗi một điểm STR tăng:"+" \n 1 sát thương vật lý."+"\n 0.5% sát thương chí mạng.";
            case StatType.Agility: return "Với mỗi một điểm AGI tăng:"+"\n 0.3% tỷ lệ chí mạng."+"\n 0.5% tỷ lệ né tránh.";
            case StatType.Intelligence: return "Với mỗi một điểm INT tăng:"+"\n 1 điểm sát thương nguyên tố."+"\n 1 điểm kháng nguyên tố."+"\n Lưu ý: nếu không sở hữu sát thương nguyên tố, chỉ số này sẽ không hỗ trợ tăng chỉ số tấn công.";
            case StatType.Vitality: return "Với mỗi một điểm VIT tăng:"+"\n 5 điểm HP tối đa."+"\n 1 điểm giáp.";
            case StatType.AttackSpeed: return "Chỉ số quyết định tốc độ tung ra đòn đánh";
            case StatType.Damage: return "Chỉ số quyết định lượng sát thương vật lý có thể gây ra. \nLượng sát thương có thể bị giảm bởi lượng giáp của mục tiêu.";
            case StatType.CritChance: return "Chỉ số quyết định xác suất một đòn đánh gây ra có thể chí mạng.";
            case StatType.CritPower: return "Chỉ số quyết định % lượng sát thương mà đòn chí mạng có thể gây ra so với đòn đánh thường.";
            case StatType.ArmorReduction: return "Chỉ số quyết định % giáp được bỏ qua của mục tiêu trong giai đoạn tính sát thương.";
            case StatType.FireDamage: return "Chỉ số quyết định lượng sát thương nguyên tố lửa. \nKhi trở thành sát thương nguyên tố chủ đạo sẽ gây hiệu ứng bỏng: Gây sát thương lên mục tiêu theo thời gian, tổng sát thương dựa trên 40% sát thương lửa.";
            case StatType.IceDamage: return "Chỉ số quyết định lượng sát thương nguyên tố băng. \nKhi trở thành sát thương nguyên tố chủ đạo sẽ gây hiệu ứng lạnh: Làm chậm hành động của mục tiêu bị ảnh hưởng.";
            case StatType.LightningDamage: return "Chỉ số quyết định lượng sát thương nguyên tố điện. \nKhi trở thành sát thương nguyên tố chủ đạo sẽ gây hiệu ứng tĩnh điện: Tích trữ điện năng lên mục tiêu, khi tích đủ điện năng sẽ gây sát thương dựa trên sát thương điện. ";
            case StatType.ElementalDamage: return "Chỉ số kết hợp giữa sát thương ba nguyên tố, với các hiệu ứng sau:"+"\n Nguyên tố có chỉ số sát thương cao nhất trở thành nguyên tố chủ đạo gây 100% sát thương, biến sát thương các đòn đánh, chiêu thức thành dạng nguyên tố đó và gây ra hiệu ứng phụ dựa trên nguyên tố chủ đạo."+"\n Các nguyên tố còn lại đóng góp 50% sát thương của chúng thành sát thương tổng.";
            case StatType.Armor: return "Chỉ số quyết định khả năng giảm sát thương vật lý nhận phải.\nLượng sát thương được giảm thực tế: "+playerStats.GetArmorMitigation(0)*100+"%, tối đa giảm 85% sát thương.";
            case StatType.EvasionChance: return "Chỉ số quyết định xác suất bỏ qua đòn sát thương nhận phải, tối đa 60%.";
            case StatType.IceResistance: return "Chỉ số quyết định lượng sát thương băng có thể giảm, tối đa 75%.";
            case StatType.FireResistance: return "Chỉ số quyết định lượng sát thương lửa có thể giảm, tối đa 75%.";
            case StatType.LightningResistance: return "Chỉ số quyết định lượng sát thương điện có thể giảm, tối đa 75%.";
            default: return "Chỉ số không rõ";
        }
    }
}
