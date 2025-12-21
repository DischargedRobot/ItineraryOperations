using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItineraryOperations.Models
{
    public class Equipment
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public int TypeOperationID { get; set; }

        [Required, ForeignKey("TypeOperationID")]
        public TypesOperations? TypeOperations { get; set; }
        public static void Felling(PostgresContext context)
        {
            context.Equipment.ExecuteDelete();
            
            var equipmentList = new List<Equipment>
            {
                // Вырубка и гидровырубка (TypeOperationID: 3, 4, 5, 6, 10)
                new Equipment { ID = 1, Name = "Вырубной пресс (механический, 25–630 т)", TypeOperationID = 3 },
                new Equipment { ID = 2, Name = "Вырубной пресс (гидравлический, до 1000 т)", TypeOperationID = 3 },
                new Equipment { ID = 3, Name = "Координатно‑пробивной станок с ЧПУ", TypeOperationID = 3 },
                new Equipment { ID = 4, Name = "Гидроабразивный станок", TypeOperationID = 10 },  // гидровырубка
                new Equipment { ID = 5, Name = "Вырубной станок «Вырубная 2000»", TypeOperationID = 4 },
                new Equipment { ID = 6, Name = "Вырубной станок «Вырубная 5000»", TypeOperationID = 5 },
                new Equipment { ID = 7, Name = "Вырубной станок «Вырубная 6000»", TypeOperationID = 6 },

                // Гибка (TypeOperationID: 9)
                new Equipment { ID = 8, Name = "Листогибочный пресс (20–400 т, длина гиба до 4 м)", TypeOperationID = 9 },
                new Equipment { ID = 9, Name = "Роликовый гибочный станок", TypeOperationID = 9 },
                new Equipment { ID = 10, Name = "Трубогиб (профилегибочный станок)", TypeOperationID = 9 },

                // Вальцовка (TypeOperationID: 2)
                new Equipment { ID = 11, Name = "Трёхвалковая листогибочная машина", TypeOperationID = 2 },
                new Equipment { ID = 12, Name = "Четырёхвалковая листогибочная машина", TypeOperationID = 2 },
                new Equipment { ID = 13, Name = "Профилегибочные вальцы", TypeOperationID = 2 },
                new Equipment { ID = 14, Name = "Кольцегибочный станок", TypeOperationID = 2 },

                // Гальваника (TypeOperationID: 8)
                new Equipment { ID = 15, Name = "Гальваническая ванна (полипропиленовая)", TypeOperationID = 8 },
                new Equipment { ID = 16, Name = "Гальваническая линия (автоматизированная)", TypeOperationID = 8 },
                new Equipment { ID = 17, Name = "Фильтровальная установка для электролита", TypeOperationID = 8 },

                // Гравировка (TypeOperationID: 11)
                new Equipment { ID = 18, Name = "Лазерный гравер (CO₂)", TypeOperationID = 11 },
                new Equipment { ID = 19, Name = "Лазерный гравер (волоконный)", TypeOperationID = 11 },
                new Equipment { ID = 20, Name = "Фрезерный станок с ЧПУ (для гравировки)", TypeOperationID = 11 },
                new Equipment { ID = 21, Name = "Электроэрозионный станок (микрогравировка)", TypeOperationID = 11 },

                // Заготовительные операции (TypeOperationID: 13, 14)
                new Equipment { ID = 22, Name = "Ленточнопильный станок", TypeOperationID = 13 },
                new Equipment { ID = 23, Name = "Плазменный резак с ЧПУ", TypeOperationID = 13 },
                new Equipment { ID = 24, Name = "Газорезательная машина (кислородно‑ацетиленовая)", TypeOperationID = 13 },
                new Equipment { ID = 25, Name = "Дисковая пила (для прутков и труб)", TypeOperationID = 13 },
                new Equipment { ID = 26, Name = "Ножницы гильотинные", TypeOperationID = 13 },
                new Equipment { ID = 27, Name = "Заготовительный станок «заготовительная ст»", TypeOperationID = 14 },

                // Вязальные операции (TypeOperationID: 7)
                new Equipment { ID = 28, Name = "Пневматический клепальный станок", TypeOperationID = 7 },
                new Equipment { ID = 29, Name = "Гидравлический клепальный станок", TypeOperationID = 7 },
                new Equipment { ID = 30, Name = "Станок для вязки проволокой (автоматический)", TypeOperationID = 7 },
                new Equipment { ID = 31, Name = "Пресс‑инструмент для замковых профилей", TypeOperationID = 7 },

                // Дефектировочные операции (TypeOperationID: 12)
                new Equipment { ID = 32, Name = "Видеоизмерительный микроскоп", TypeOperationID = 12 },
                new Equipment { ID = 33, Name = "Ультразвуковой дефектоскоп", TypeOperationID = 12 },
                new Equipment { ID = 34, Name = "Магнитопорошковый дефектоскоп", TypeOperationID = 12 },
                new Equipment { ID = 35, Name = "Калибры и шаблоны (контрольно‑измерительные)", TypeOperationID = 12 },

                // АДС (TypeOperationID: 1) — предположим, это специализированный агрегат
                new Equipment { ID = 36, Name = "Агрегат «адс» (специализированный)", TypeOperationID = 1 }
            };

            context.AddRange(equipmentList);
            context.SaveChanges();
        }
    }
}
