using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
namespace WindowsFormsApp1
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
    public static class FileUtils
    {
        public static string ReadFileContent(string path)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory+"Base/Text", path);
            string fileContent;

            using (StreamReader sr = new StreamReader(filePath))
            {
                fileContent = sr.ReadToEnd();
            }

            return fileContent;
        }
    }
    interface GetInf
    {
        void GetInfo(Form1 form);
    }
    interface SetPic
    {
        void SetPic(Form1 form);
    }
    public abstract class Animal:GetInf,SetPic
    {
        public virtual int maxday_withoutfood { get; set; }
        public int days_withoutfood = 0;
        public virtual double perFood { get; set; }
        public virtual string Food { get; set; }
        public double death_chance  = 0;

        public string kingdom = "животных";
        public virtual string animal_class { get; set; }
        public virtual string pathclass { get; set; }
        public virtual string family { get; set; }
        public virtual string pathfamily { get; set; }
        public virtual string species { get; set; }
        public virtual string pathspecies { get; set; }
        public void GetInfo(Form1 form)
        {

            form.textBox1.Text = species + " принадлежат к царству " + kingdom + ", классу" + animal_class + " "+ family +
                ". Этот класс обладает следующим свойствами:\n" + Environment.NewLine + FileUtils.ReadFileContent(pathclass) +
                "Для " + family + " характерны следующие особенности:\n"+ Environment.NewLine + FileUtils.ReadFileContent(pathfamily) +
                species + " обладают следующими свойствами"+ Environment.NewLine + FileUtils.ReadFileContent(pathspecies);

        }
        public void SetPic(Form1 form)
        {
            form.pictureBox1.Image = new Bitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Base/Pic", species+".jpg"));
        }
        public virtual Eating Eat { get; set; }

    }
    public abstract class Mammals:Animal
    {
        public override Eating Eat { get; set; } = Eats.EatPlant;
        public override int maxday_withoutfood { get; set; } = 3;
        public override string Food { get; set; } = "Растительная пища";
        public override string animal_class { get; set; } = " Млекопитающих";
        public override string pathclass { get; set; } = "mammals_properties.txt";
    }
    public abstract class Reptiles : Animal
    {
        public override Eating Eat { get; set; } = Eats.EatMeat;
        public override string Food { get; set; } = "Мясо";
        public override int maxday_withoutfood { get; set; } = 7;
        public override string animal_class { get; set; } = " Рептилий";
        public override string pathclass { get; set; } = "reptiles_properties.txt";
    }
    public abstract class Bird : Animal
    {
        public override int maxday_withoutfood { get; set; } = 2;
        public override string Food { get; set; } = "Морепродукты";
        public override Eating Eat { get; set; } = Eats.EatFish;
        public override string animal_class { get; set; } = " Птиц";
        public override string pathclass { get; set; } = "bird_properties.txt";

    }
    public abstract class Herons: Bird
    {
        public override double perFood { get; set; } = 0.3;
        public override string family { get; set; } = "семейства Цаплевых";
        public override string pathfamily { get; set; } = "herons_properties.txt";
    }
    public class Rufescent_Tiger_Heron  : Herons
    {
        public override string species { get; set; } = "Мраморные тигровые цапли";
        public override string pathspecies { get; set; } = "tigerherons_properties.txt";
    }
    public class Oatbilled_Heron : Herons
    {
        public override string species { get; set; } = "Челноклювы";
        public override string pathspecies { get; set; } = "oatbilledherons_properties.txt";
    }
    public class Goliath_Heron : Herons
    {
        public override string species { get; set; } = "Цапля-голиафы";
        public override string pathspecies { get; set; } = "goliathherons_properties.txt";
    }
    public abstract class Penguins : Bird
    {
        public override double perFood { get; set; } = 5;
        public override string family { get; set; } = "семейства Пингвиновые";
        public override string pathfamily { get; set; } = "penguins_properties.txt";
    }
    public abstract class Accipitridae : Bird
    {
        public override Eating Eat { get; set; } = Eats.EatMeat;
        public override string Food { get; set; } = "Мясо";
        public override string family { get; set; } = "семейству Ястребиные";
        public override string pathfamily { get; set; } = "accipitridae_properties.txt";
    }
    public class Pernis_Accipitridae : Accipitridae
    {
        public override double perFood { get; set; } = 0.1;
        public override string species { get; set; } = "Осоеды";
        public override string pathspecies { get; set; } = "pernis_properties.txt";
    }
    public class Bearded_Vultures  : Accipitridae
    {
        public override double perFood { get; set; } = 0.2;
        public override string species { get; set; } = "Бородачи";
        public override string pathspecies { get; set; } = "beardedvultures_properties.txt";
    }
    public class Circaetinae_Accipitridaes : Accipitridae
    {
        public override double perFood { get; set; } = 0.15;
        public override string species { get; set; } = "Змееяды";
        public override string pathspecies { get; set; } = "Circaetinae_Accipitridaes.txt";
    }
    public class Emperor_Penguins:Penguins
    {
        public override string species { get; set; } = "Императорские пингвины";
        public override string pathspecies { get; set; } = "Emperor_Penguins.txt";
    }
    public class Snares_Penguins : Penguins
    {
        public override string species { get; set; } = "Снэрские хохлатые пингвины";
        public override string pathspecies { get; set; } = "Snares_Penguins.txt";
    }
    public class YellowEyed_Penguins : Penguins
    {
        public override string species { get; set; } = "Желтоглазые пингвины";
        public override string pathspecies { get; set; } = "YellowEyed_Penguins.txt";
    }
    public abstract class Turtles : Reptiles
    {
        public override Eating Eat { get; set; } = Eats.EatFish;
        public override string Food { get; set; } = "Морепродукты";
        public override int maxday_withoutfood { get; set; } = 3;
        public override string family { get; set; } = "отряда Черепахи";
        public override string pathfamily { get; set; } = "Turtles.txt";
    }
    public abstract class Сrocodilians:Reptiles
    {
        public override double perFood { get; set; } = 15;
        public override string family { get; set; } = "отряда Крокодилы";
        public override string pathfamily { get; set; } = "Сrocodilians.txt";

    }
    public abstract class Snakes : Reptiles
    {
        public override int maxday_withoutfood { get; set; } = 14;
        public override string family { get; set; } = "подотряда Змеи";
        public override string pathfamily { get; set; } = "Snakes.txt";
    }
    public class SideWinder_RattleSnakes:Snakes
    {
        public override double perFood { get; set; } = 0.5;
        public override string species { get; set; } = "Рогатые гремучники";
        public override string pathspecies { get; set; } = "SideWinder_RattleSnakes.txt";
    }
    public class Indian_Cobras : Snakes
    {
        public override double perFood { get; set; } = 0.6;
        public override string species { get; set; } = "Индийские кобры";
        public override string pathspecies { get; set; } = "Indian_Cobras.txt";
    }
    public class Reticulated_Pythons: Snakes
    {
        public override double perFood { get; set; } = 13;
        public override string species { get; set; } = "Сетчатые питоны";
        public override string pathspecies { get; set; } = "Reticulated_Pythons.txt";
    }
    public class SaltWater_Crocodiles : Сrocodilians
    {
        public override string species { get; set; } = "Гребнистые крокодилы";
        public override string pathspecies { get; set; } = "SaltWater_Crocodiles.txt";
    }
    public class American_Alligators: Сrocodilians
    {
        public override string species { get; set; } = "Миссисипские аллигаторы";
        public override string pathspecies { get; set; } = "American_Alligators.txt";
    }
    public class Gharials: Сrocodilians
    {
        public override string species { get; set; } = "Гангские гавиалы";
        public override string pathspecies { get; set; } = "Gharials.txt";
    }
    public class Galapagos_Turtles:Turtles
    {
        public override Eating Eat { get; set; } = Eats.EatPlant;
        public override double perFood { get; set; } = 4;
        public override string Food { get; set; } = "Растительная пища";
        public override string species { get; set; } = "Слоновые черепахи";
        public override string pathspecies { get; set; } = "Galapagos_Turtles.txt";
    }
    public class PondSlider_Turtles : Turtles
    {
        public override double perFood { get; set; } = 0.01;
        public override string species { get; set; } = "Красноухие черепахи";
        public override string pathspecies { get; set; } = "PondSlider_Turtles.txt";
    }
    public class AlligatorSnapping_Turtles : Turtles
    {
        public override double perFood { get; set; } = 2;
        public override string species { get; set; } = "Грифовые черепахи";
        public override string pathspecies { get; set; } = "AlligatorSnapping_Turtles.txt";
    }
    public abstract class Felidae:Mammals
    {
        public override double perFood { get; set; } = 8;
        public override Eating Eat { get; set; } = Eats.EatMeat;
        public override string Food { get; set; } = "Мясо";
        public override string family { get; set; } = "семейства Кошачьих";
        public override string pathfamily { get; set; } = "Felidae.txt";
    }
    public abstract class Elephantidae:Mammals
    {
        public override double perFood { get; set; } = 50;
        public override string family { get; set; } = "семейства Слоновых";
        public override string pathfamily { get; set; } = "Elephantidae.txt";
    }
    public abstract class Primates:Mammals
    {
        public override string family { get; set; } = "отряда Приматы";
        public override string pathfamily { get; set; } = "Primates.txt";
    }
    public class Lemurs:Primates
    {
        public override double perFood { get; set; } = 0.3;
        public override string species { get; set; } = "Лемуры";
        public override string pathspecies { get; set; } = "Lemurs.txt";
    }
    public class Gorillas : Primates
    {
        public override double perFood { get; set; } = 20;
        public override string species { get; set; } = "Гориллы";
        public override string pathspecies { get; set; } = "Gorillas.txt";
    }
    public class Chimpanzee: Primates
    {
        public override double perFood { get; set; } = 4;
        public override string species { get; set; } = "Шимпанзе";
        public override string pathspecies { get; set; } = "Chimpanzee.txt";
    }
    public class Savanna_Elephants:Elephantidae
    {
        public override string species { get; set; } = "Саванные слоны";
        public override string pathspecies { get; set; } = "Savanna_Elephants.txt";
    }
    public class Indian_Elephants : Elephantidae
    {
        public override string species { get; set; } = "Индийские слоны";
        public override string pathspecies { get; set; } = "Indian_Elephants.txt";
    }
    public class Forest_Elephants:Elephantidae
    {
        public override string species { get; set; } = "Лесные слоны";
        public override string pathspecies { get; set; } = "Forest_Elephants.txt";
    }
    public class Lions : Felidae
    {
        public override string species { get; set; } = "Львы";
        public override string pathspecies { get; set; } = "Lions.txt";
    }
    public class Tigers:Felidae
    {
        public override string species { get; set; } = "Тигры";
        public override string pathspecies { get; set; } = "Tigers.txt";
    }
    public class Cheetahs:Felidae
    {
        public override double perFood { get; set; } = 4;
        public override string species { get; set; } = "Гепарды";
        public override string pathspecies { get; set; } = "Cheetahs.txt";
    }
    public class AnimalFactory
    {
        private static Dictionary<string, Func<Animal>> animalConstructors = new Dictionary<string, Func<Animal>>
    {
        { "Лев", () => new Lions() },
        { "Тигр", () => new Tigers() },
        { "Саванный слон", () => new Savanna_Elephants() },
        { "Гепард", () => new Cheetahs() },
        { "Индийский слон", () => new Indian_Elephants() },
        { "Лесной слон", () => new Forest_Elephants() },
        { "Шимпанзе", () => new Chimpanzee() },
        { "Горилла", () => new Gorillas() },
        { "Лемур", () => new Lemurs() },
        { "Грифовая черепаха", () => new AlligatorSnapping_Turtles() },
        { "Красноухая черепаха", () => new PondSlider_Turtles() },
        { "Слоновая черепаха", () => new Galapagos_Turtles() },
        { "Гангский гавиал", () => new Gharials() },
        { "Миссисипский аллигатор", () => new American_Alligators() },
        { "Гребнистый крокодил", () => new SaltWater_Crocodiles() },
        { "Сетчатый питон", () => new Reticulated_Pythons() },
        { "Индийская кобра", () => new Indian_Cobras() },
        { "Рогатый гремучник", () => new SideWinder_RattleSnakes() },
        { "Снэрский хохлатый пингвин", () => new Snares_Penguins() },
        { "Желтоглазый пингвин", () => new YellowEyed_Penguins() },
        { "Императорский пингвин", () => new Emperor_Penguins() },
        { "Змееяд", () => new Circaetinae_Accipitridaes() },
        { "Бородач", () => new Bearded_Vultures() },
        { "Осоед", () => new Pernis_Accipitridae() },
        { "Цапля-голиаф", () => new Goliath_Heron() },
        { "Челноклюв", () => new Oatbilled_Heron() },
        { "Мраморная тигровая цапля", () => new Rufescent_Tiger_Heron() },

    };

        public static Animal CreateAnimal(string animalType)
        {
            if (animalConstructors.TryGetValue(animalType, out Func<Animal> constructor))
            {
                return constructor();
            }
            else
            {
                throw new ArgumentException("Неизвестный тип животного");
            }
        }
    }
    public class Enclosure
    {
        public List<Animal> Animals { get; set; }
        private static ZOO zooInstance = ZOO.Instance;
        private Func<double> getStorage;
        private Dictionary<string, Func<double>> variants = new Dictionary<string, Func<double>>()
        {
            { "Мясо", () => zooInstance.Meat },
            { "Морепродукты", () => zooInstance.Fish },
            { "Растительная пища",() => zooInstance.Plant_Food }
        };
        public string FoodType { get; set; }
        public string Species;
        public double FoodperAnimal;
        public double needFood;
        public double storage;
        public int maxdayswithout;
        public int countFeedAnimals;
        public Enclosure(string species)
        {
            Species = species;
            Animals = new List<Animal>();
            countFeedAnimals = 0;
        }

        public void AddAnimal(Animal animal)
        {
            Animals.Add(animal);
            FoodType = animal.Food;
            FoodperAnimal = animal.perFood;
            maxdayswithout = animal.maxday_withoutfood;
            variants.TryGetValue(FoodType, out getStorage);
            storage = getStorage();
        }
        public string[] GetInfo()
        {
            needFood = Animals.Count() * FoodperAnimal;
            string[] data = new string[] { Species, Animals.Count().ToString(), FoodType +" | "+ FoodperAnimal.ToString(), countFeedAnimals.ToString() };
            return data;
        }
        public void Feed()
        {
            storage = getStorage();
            int i = countFeedAnimals;
            while (i < Animals.Count() && (getStorage() - FoodperAnimal) >= 0)
            {
                Animals[i].Eat(FoodperAnimal);
                Animals[i].days_withoutfood = 0;
                i++;
            }
            countFeedAnimals = i;
        }
        public int CheckDeath(Form1 form)
        {
            ZOO.Instance.money += FoodperAnimal * Animals.Count() / maxdayswithout*10;
            int countofdeath = 0;
            for (int i=countFeedAnimals;i<Animals.Count();i++)
            {
                Animals[i].days_withoutfood++;
                double deathChance = Math.Exp((Animals[i].days_withoutfood - Animals[i].maxday_withoutfood) / (Animals[i].maxday_withoutfood + 1.0))-1;
                Animals[i].death_chance = deathChance < 0 ? 0 : deathChance;
                Random random = new Random();
                //double d = Math.Round(animal.death_chance, 2);
                double randomDouble = random.NextDouble();
                if (randomDouble <= Animals[i].death_chance)
                {
                    Animals.Remove(Animals[i]);
                    countofdeath++;
                }
            }
            countFeedAnimals = 0;
            return countofdeath;
        }
    }
    public sealed class ZOO
    {
        private static ZOO instance = null;
        private static readonly object padlock = new object();
        public double money;
        Dictionary<string, int> listofanimals = new Dictionary<string, int>();
        Dictionary<string, Tuple<double, Action<double>>> variantstobuy;

        private ZOO()
        {
            Enclosures = new List<Enclosure>();
            Meat = 0;
            Plant_Food = 0;
            Fish = 0;
            money = 50000;
            countEnclos = 0;
            countAnimals = 0;
            variantstobuy = new Dictionary<string, Tuple<double, Action<double>>>()
    {
        { "Мясо (100)", new Tuple<double, Action<double>>(100, amount => Meat += amount) },
        { "Морепродукты (50)", new Tuple<double, Action<double>>(50, amount => Fish += amount) },
        { "Растительная пища (20)", new Tuple<double, Action<double>>(20, amount => Plant_Food += amount) }
    };
        }

        public static ZOO Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ZOO();
                    }
                    return instance;
                }
            }
        }
        private List<Enclosure> Enclosures;
        public double Meat;
        public double Plant_Food;
        public double Fish;
        private int countAnimals;
        private int maxcountAnimals=50;
        private int countEnclos;
        public string BuyFood(string type)
        {
            variantstobuy.TryGetValue(type, out var foodInfo);
            if (money- foodInfo.Item1 >= 0) 
            {
                foodInfo.Item2(50);
                money -= foodInfo.Item1;
                return "Удачно куплено";
            }
            else
            {
                return "Не хватило денег на " + type;
            }
        }
        public void GetInfo(Form1 form)
        {
            foreach (object item in form.comboBox2.Items)
            {
                int coord;
                listofanimals.TryGetValue(item.ToString(), out coord);
                string[] myArray = Enclosures[coord].GetInfo();
                form.dataGridView1.Rows[coord].Cells[0].Value = coord+1;
                for (int i = 0; i < 4; i++)
                {
                    form.dataGridView1.Rows[coord].Cells[i + 1].Value = myArray[i];
                }
            }
        }
        public void Feed_All(Form1 form)
        {
            foreach (object item in form.comboBox2.Items)
            {
                int coord;
                listofanimals.TryGetValue(item.ToString(), out coord);
                Enclosures[coord].Feed();
                GetInfo(form);
            }
        }
        public void Add_Animals(Form1 form, string selectedItem)
        {
            if (countAnimals < maxcountAnimals) 
            {
                Animal animal = AnimalFactory.CreateAnimal(selectedItem);
                if (!listofanimals.ContainsKey(selectedItem))
                {
                    listofanimals.Add(selectedItem, countEnclos);
                    Enclosure TempEnclosure = new Enclosure(selectedItem);
                    Enclosures.Add(TempEnclosure);
                    Enclosures[countEnclos].AddAnimal(animal);
                    form.comboBox2.Items.Add(selectedItem);
                    form.dataGridView1.Rows.Add(countEnclos + 1, selectedItem, 1, animal.Food + " | " + animal.perFood, Enclosures[countEnclos].countFeedAnimals);
                    countEnclos++;
                }
                else
                {
                    int coord;
                    listofanimals.TryGetValue(selectedItem, out coord);
                    Enclosures[coord].Animals.Add(animal);
                    form.dataGridView1.Rows[coord].Cells[0].Value = coord + 1;
                    GetInfo(form);
                }
                MessageBox.Show("Успешно добавлен: " + selectedItem, "Добавление животного");
            }
            else
            {
                MessageBox.Show("Достигнут предел по числу животных" + selectedItem, "Добавление животного");
            }
        }
        public void Next_Day(Form1 form)
        {
            form.textBox3.Text = "";
            for (int i = 0; i < Enclosures.Count(); i++)
            {
                int ret = Enclosures[i].CheckDeath(form);
                if (ret > 0)
                {
                    form.textBox3.Text += "В вольере № " + (i + 1).ToString() + " умерло: " + ret + Environment.NewLine;
                }
                Enclosures[i].countFeedAnimals = 0;
                GetInfo(form);
            }
        }
    }
    public delegate void Eating(double count);
    public class Eats
    {
        public static void EatMeat(double count)
        {
            ZOO.Instance.Meat -= count;
        }
        public static void EatPlant(double count)
        {
            ZOO.Instance.Plant_Food -= count;
        }
        public static void EatFish(double count)
        {
            ZOO.Instance.Fish -= count;
        }
    }
}

