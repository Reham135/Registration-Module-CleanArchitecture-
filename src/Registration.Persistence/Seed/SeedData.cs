using Registration.Domain.Entities;

namespace Registration.Persistence.Seed;

/// <summary>
/// Fixed-id seed data for address lookup tables (governorates and cities), used with
/// EF Core's HasData so that ids are deterministic across environments.
/// </summary>
public static class SeedData
{
    public static IReadOnlyList<Governorate> Governorates { get; } = new List<Governorate>
    {
        new(1, "Cairo"),
        new(2, "Giza"),
        new(3, "Alexandria"),
        new(4, "Qalyubia"),
        new(5, "Sharqia"),
        new(6, "Dakahlia"),
        new(7, "Beheira"),
        new(8, "Minya"),
        new(9, "Qena"),
        new(10, "Sohag"),
        new(11, "Assiut"),
        new(12, "Faiyum"),
        new(13, "Gharbia"),
        new(14, "Monufia"),
        new(15, "Beni Suef"),
        new(16, "Damietta"),
        new(17, "Port Said"),
        new(18, "Ismailia"),
        new(19, "Suez"),
        new(20, "Aswan"),
        new(21, "Luxor"),
        new(22, "Red Sea"),
        new(23, "New Valley"),
        new(24, "Matrouh"),
        new(25, "North Sinai"),
        new(26, "South Sinai"),
        new(27, "Kafr El Sheikh"),
    };

    public static IReadOnlyList<City> Cities { get; } = new List<City>
    {
        // Cairo
        new(101, "Nasr City", governorateId: 1),
        new(102, "Maadi", governorateId: 1),
        new(103, "Heliopolis", governorateId: 1),
        new(104, "Downtown Cairo", governorateId: 1),
        new(105, "Helwan", governorateId: 1),
        new(106, "Shubra", governorateId: 1),
        new(107, "New Cairo", governorateId: 1),
        new(108, "Madinaty", governorateId: 1),

        // Giza
        new(201, "Dokki", governorateId: 2),
        new(202, "Mohandessin", governorateId: 2),
        new(203, "6th of October", governorateId: 2),
        new(204, "Haram", governorateId: 2),
        new(205, "Imbaba", governorateId: 2),
        new(206, "Faisal", governorateId: 2),
        new(207, "Sheikh Zayed", governorateId: 2),
        new(208, "Badrasheen", governorateId: 2),

        // Alexandria
        new(301, "Sidi Gaber", governorateId: 3),
        new(302, "Smouha", governorateId: 3),
        new(303, "Miami", governorateId: 3),
        new(304, "Montaza", governorateId: 3),
        new(305, "Borg El Arab", governorateId: 3),
        new(306, "Agami", governorateId: 3),
        new(307, "Sidi Bishr", governorateId: 3),
        new(308, "Mansheya", governorateId: 3),

        // Qalyubia
        new(401, "Banha", governorateId: 4),
        new(402, "Shubra El-Kheima", governorateId: 4),
        new(403, "Qalyub", governorateId: 4),
        new(404, "Qaha", governorateId: 4),
        new(405, "Khanka", governorateId: 4),
        new(406, "Tukh", governorateId: 4),
        new(407, "Kafr Shukr", governorateId: 4),
        new(408, "Obour", governorateId: 4),

        // Sharqia
        new(501, "Zagazig", governorateId: 5),
        new(502, "10th of Ramadan City", governorateId: 5),
        new(503, "Belbeis", governorateId: 5),
        new(504, "Abu Hammad", governorateId: 5),
        new(505, "Faqous", governorateId: 5),
        new(506, "Minya Al Qamh", governorateId: 5),
        new(507, "Husseiniya", governorateId: 5),
        new(508, "Diyarb Negm", governorateId: 5),

        // Dakahlia
        new(601, "Mansoura", governorateId: 6),
        new(602, "Talkha", governorateId: 6),
        new(603, "Mit Ghamr", governorateId: 6),
        new(604, "Dikirnis", governorateId: 6),
        new(605, "Aga", governorateId: 6),
        new(606, "Belqas", governorateId: 6),
        new(607, "Sherbin", governorateId: 6),
        new(608, "Mit Salsil", governorateId: 6),

        // Beheira
        new(701, "Damanhur", governorateId: 7),
        new(702, "Kafr El Dawwar", governorateId: 7),
        new(703, "Rashid", governorateId: 7),
        new(704, "Edku", governorateId: 7),
        new(705, "Abu Hummus", governorateId: 7),
        new(706, "Itay El Baroud", governorateId: 7),
        new(707, "Hosh Issa", governorateId: 7),
        new(708, "Kom Hamada", governorateId: 7),

        // Minya
        new(801, "Minya", governorateId: 8),
        new(802, "Mallawi", governorateId: 8),
        new(803, "Beni Mazar", governorateId: 8),
        new(804, "Maghagha", governorateId: 8),
        new(805, "Samalut", governorateId: 8),
        new(806, "Matai", governorateId: 8),
        new(807, "Abu Qurqas", governorateId: 8),
        new(808, "Deir Mawas", governorateId: 8),

        // Qena
        new(901, "Qena", governorateId: 9),
        new(902, "Naqada", governorateId: 9),
        new(903, "Qus", governorateId: 9),
        new(904, "Nag Hammadi", governorateId: 9),
        new(905, "Dishna", governorateId: 9),
        new(906, "Abu Tesht", governorateId: 9),
        new(907, "Farshut", governorateId: 9),
        new(908, "Qift", governorateId: 9),

        // Sohag
        new(1001, "Sohag", governorateId: 10),
        new(1002, "Akhmim", governorateId: 10),
        new(1003, "Girga", governorateId: 10),
        new(1004, "Tahta", governorateId: 10),
        new(1005, "Tama", governorateId: 10),
        new(1006, "Maragha", governorateId: 10),
        new(1007, "El Balyana", governorateId: 10),
        new(1008, "Dar El Salam", governorateId: 10),

        // Assiut
        new(1101, "Assiut", governorateId: 11),
        new(1102, "Dairut", governorateId: 11),
        new(1103, "Manfalut", governorateId: 11),
        new(1104, "Abnub", governorateId: 11),
        new(1105, "Abu Tig", governorateId: 11),
        new(1106, "Sahel Selim", governorateId: 11),
        new(1107, "Al Ghanayem", governorateId: 11),
        new(1108, "Al Badari", governorateId: 11),

        // Faiyum
        new(1201, "Faiyum", governorateId: 12),
        new(1202, "Ibsheway", governorateId: 12),
        new(1203, "Senuris", governorateId: 12),
        new(1204, "Tamiya", governorateId: 12),
        new(1205, "Itsa", governorateId: 12),
        new(1206, "Yousef El Seddik", governorateId: 12),

        // Gharbia
        new(1301, "Tanta", governorateId: 13),
        new(1302, "El Mahalla El Kubra", governorateId: 13),
        new(1303, "Kafr El Zayat", governorateId: 13),
        new(1304, "Zefta", governorateId: 13),
        new(1305, "Samanoud", governorateId: 13),
        new(1306, "Qutour", governorateId: 13),
        new(1307, "Basyoun", governorateId: 13),
        new(1308, "El Santa", governorateId: 13),

        // Monufia
        new(1401, "Shibin El Kom", governorateId: 14),
        new(1402, "Sadat City", governorateId: 14),
        new(1403, "Menouf", governorateId: 14),
        new(1404, "Ashmoun", governorateId: 14),
        new(1405, "Quesna", governorateId: 14),
        new(1406, "Tala", governorateId: 14),
        new(1407, "Berket El Sab", governorateId: 14),
        new(1408, "El Bagour", governorateId: 14),

        // Beni Suef
        new(1501, "Beni Suef", governorateId: 15),
        new(1502, "Al Wasta", governorateId: 15),
        new(1503, "Nasser", governorateId: 15),
        new(1504, "Ihnasia", governorateId: 15),
        new(1505, "Biba", governorateId: 15),
        new(1506, "Al Fashn", governorateId: 15),
        new(1507, "Sumusta", governorateId: 15),

        // Damietta
        new(1601, "Damietta", governorateId: 16),
        new(1602, "New Damietta", governorateId: 16),
        new(1603, "Faraskur", governorateId: 16),
        new(1604, "Kafr Saad", governorateId: 16),
        new(1605, "Zarqa", governorateId: 16),
        new(1606, "Ras El Bar", governorateId: 16),
        new(1607, "Kafr El Batikh", governorateId: 16),

        // Port Said
        new(1701, "Port Said", governorateId: 17),
        new(1702, "Port Fouad", governorateId: 17),
        new(1703, "Al Dawahy", governorateId: 17),
        new(1704, "Al Zohour", governorateId: 17),
        new(1705, "Al Arab", governorateId: 17),
        new(1706, "Al Manakh", governorateId: 17),

        // Ismailia
        new(1801, "Ismailia", governorateId: 18),
        new(1802, "Fayed", governorateId: 18),
        new(1803, "Qantara Sharq", governorateId: 18),
        new(1804, "Qantara Gharb", governorateId: 18),
        new(1805, "Tal El Kebir", governorateId: 18),
        new(1806, "Abu Suwir", governorateId: 18),

        // Suez
        new(1901, "Suez", governorateId: 19),
        new(1902, "Ataqah", governorateId: 19),
        new(1903, "Arbaeen", governorateId: 19),
        new(1904, "Faisal", governorateId: 19),
        new(1905, "Ganayen", governorateId: 19),

        // Aswan
        new(2001, "Aswan", governorateId: 20),
        new(2002, "Kom Ombo", governorateId: 20),
        new(2003, "Edfu", governorateId: 20),
        new(2004, "Daraw", governorateId: 20),
        new(2005, "Nasr El Nuba", governorateId: 20),
        new(2006, "Abu Simbel", governorateId: 20),
        new(2007, "Kalabsha", governorateId: 20),

        // Luxor
        new(2101, "Luxor", governorateId: 21),
        new(2102, "Esna", governorateId: 21),
        new(2103, "Armant", governorateId: 21),
        new(2104, "Tod", governorateId: 21),
        new(2105, "Qurna", governorateId: 21),
        new(2106, "Bayadeyah", governorateId: 21),

        // Red Sea
        new(2201, "Hurghada", governorateId: 22),
        new(2202, "Safaga", governorateId: 22),
        new(2203, "El Qoseir", governorateId: 22),
        new(2204, "Marsa Alam", governorateId: 22),
        new(2205, "Ras Ghareb", governorateId: 22),
        new(2206, "Shalateen", governorateId: 22),

        // New Valley
        new(2301, "Kharga", governorateId: 23),
        new(2302, "Dakhla", governorateId: 23),
        new(2303, "Farafra", governorateId: 23),
        new(2304, "Paris", governorateId: 23),
        new(2305, "Balat", governorateId: 23),

        // Matrouh
        new(2401, "Marsa Matrouh", governorateId: 24),
        new(2402, "El Hammam", governorateId: 24),
        new(2403, "El Dabaa", governorateId: 24),
        new(2404, "Sidi Barrani", governorateId: 24),
        new(2405, "Siwa", governorateId: 24),
        new(2406, "El Negila", governorateId: 24),

        // North Sinai
        new(2501, "Arish", governorateId: 25),
        new(2502, "Sheikh Zuweid", governorateId: 25),
        new(2503, "Rafah", governorateId: 25),
        new(2504, "Bir al-Abd", governorateId: 25),
        new(2505, "Nakhl", governorateId: 25),
        new(2506, "Hasana", governorateId: 25),

        // South Sinai
        new(2601, "Sharm El Sheikh", governorateId: 26),
        new(2602, "Dahab", governorateId: 26),
        new(2603, "Tor Sinai", governorateId: 26),
        new(2604, "Nuweiba", governorateId: 26),
        new(2605, "Saint Catherine", governorateId: 26),
        new(2606, "Taba", governorateId: 26),

        // Kafr El Sheikh
        new(2701, "Kafr El Sheikh", governorateId: 27),
        new(2702, "Desouk", governorateId: 27),
        new(2703, "Fuwwah", governorateId: 27),
        new(2704, "Qallin", governorateId: 27),
        new(2705, "Sidi Salem", governorateId: 27),
        new(2706, "Baltim", governorateId: 27),
        new(2707, "Hamoul", governorateId: 27),
        new(2708, "Mutubas", governorateId: 27),
    };
}
