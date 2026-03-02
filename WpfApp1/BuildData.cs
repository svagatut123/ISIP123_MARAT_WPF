using System.Collections.Generic;
using System.Linq;

namespace WpfApp1
{
    static class BuildData
    {
        public static Dictionary<int, basepart_> SelectedParts { get; set; } = new Dictionary<int, basepart_>();

        public static decimal GetTotalPrice()
        {
            return SelectedParts.Values.Sum(p => p.price ?? 0m);
        }

        public static void Clear()
        {
            SelectedParts.Clear();
        }

        public static List<string> CheckCompatibility()
        {
            var errors = new List<string>();
            var parts = SelectedParts.Values;

            var cpu = parts.FirstOrDefault(p => p.parttypeid == 1);
            var mobo = parts.FirstOrDefault(p => p.parttypeid == 4);
            var ram = parts.FirstOrDefault(p => p.parttypeid == 3);
            var cooler = parts.FirstOrDefault(p => p.parttypeid == 7);
            var case_ = parts.FirstOrDefault(p => p.parttypeid == 5);
            var psu = parts.FirstOrDefault(p => p.parttypeid == 6);
            var gpu = parts.FirstOrDefault(p => p.parttypeid == 2);

            if (cpu != null && mobo != null)
            {
                var cpuData = Core.Context.cpu_.FirstOrDefault(c => c.id == cpu.id);
                var moboData = Core.Context.motherboard_.FirstOrDefault(m => m.id == mobo.id);
                if (cpuData != null && moboData != null && cpuData.socketid != moboData.socketid)
                    errors.Add("Сокет процессора не совместим с материнской платой");
            }

            if (cpu != null && cooler != null)
            {
                var cpuData = Core.Context.cpu_.FirstOrDefault(c => c.id == cpu.id);
                if (cpuData != null)
                {
                    var compatible = Core.Context.socketprocessorcooler_.Any(spc =>
                        spc.socketid == cpuData.socketid && spc.processorcoolerid == cooler.id);
                    if (!compatible) errors.Add("Кулер не совместим с сокетом процессора");
                }
            }

            if (mobo != null && case_ != null)
            {
                var moboData = Core.Context.motherboard_.FirstOrDefault(m => m.id == mobo.id);
                if (moboData != null)
                {
                    var compatible = Core.Context.boardformfactorcase_.Any(bffc =>
                        bffc.caseid == case_.id && bffc.formfactorid == moboData.formfactorid);
                    if (!compatible) errors.Add("Форм-фактор платы не совместим с корпусом");
                }
            }

            if (mobo != null && ram != null)
            {
                var moboData = Core.Context.motherboard_.FirstOrDefault(m => m.id == mobo.id);
                var ramData = Core.Context.ram_.FirstOrDefault(r => r.id == ram.id);
                if (moboData != null && ramData != null && moboData.memorytypeid != ramData.memorytypeid)
                    errors.Add("Тип памяти платы не совместим с оперативной памятью");
            }

            if (psu != null && gpu != null)
            {
                var psuData = Core.Context.powersupply_.FirstOrDefault(p => p.id == psu.id);
                var gpuData = Core.Context.gpu_.FirstOrDefault(g => g.id == gpu.id);
                if (psuData != null && gpuData != null && gpuData.recommendpower.HasValue)
                {
                    int cpuPower = 0;
                    if (cpu != null)
                    {
                        var cpuData = Core.Context.cpu_.FirstOrDefault(c => c.id == cpu.id);
                        if (cpuData != null) cpuPower = cpuData.thermalpower;
                    }
                    int totalNeeded = gpuData.recommendpower.Value + cpuPower + 100;
                    if (psuData.power < totalNeeded)
                        errors.Add($"Мощность БП недостаточна. Нужно минимум {totalNeeded}W");
                }
            }

            return errors;
        }

        public static bool SaveAssembly(string name, string author)
        {
            try
            {
                var assembly = new assembly_ { name = name, author = author };
                Core.Context.assembly_.Add(assembly);
                Core.Context.SaveChanges();

                foreach (var part in SelectedParts.Values)
                {
                    Core.Context.partassembly_.Add(new partassembly_ { assemblyid = assembly.id, partid = part.id });
                }
                Core.Context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}