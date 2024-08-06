namespace PEPRN231_SU24_009909_LamMinhDang_BE.ViewModel
{
    public class AddDTO
    {
        //public string FootballPlayerId { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Achievements { get; set; } = null!;
        public DateTime? Birthday { get; set; }
        public string PlayerExperiences { get; set; } = null!;
        public string Nomination { get; set; } = null!;
        public string? FootballClubId { get; set; }
    }
}
