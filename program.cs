using System;

    public class Entity
    {
        private string _name;
        private string _type;
        private int _health;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }

        public Entity(string name, string type, int health = 0)
        {
            _name = name;       // fixed: was assigning property back to itself
            _type = type;
            _health = health;
        }

        public void attack()
        {
            Console.WriteLine($"{_name} attacks!");
        }

        public void Defend()
        {
            Console.WriteLine($"{_name} defends!");
        }

        public void TakeDamage(int damage)  // fixed: was referencing 'Damage' which doesn't exist here
        {
            _health -= damage;
            Console.WriteLine($"{_name} takes {damage} damage and has {_health} hp left.");
        }
    }

    public class Player : Entity
    {
        private string _potion;
        private int _attackDamage;  // fixed: was string, should be int

        public string SelectedPotion
        {
            get { return _potion; }
            set { _potion = value; }
        }

        public int AttackDamage
        {
            get { return _attackDamage; }
            set { _attackDamage = value; }
        }

        public Player(string name, string type, int health, string potion) : base(name, type, health)
        {
            _potion = potion;
            _attackDamage = 15;
        }

        public void UsePotion(Potion potion)    // fixed: now accepts a Potion object
        {
            Console.WriteLine($"{Name} uses {potion.Name}! {potion.PotionEffect}");
            potion.PotionActive();
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Player Name: {Name}");
            Console.WriteLine($"Player Type: {Type}");
            Console.WriteLine($"Player Health: {Health}");
            Console.WriteLine($"Selected Potion: {SelectedPotion}");
        }
    }

    public class Enemy : Entity
    {
        private int _damage;

        public int EnemyDamage
        {
            get { return _damage; }
            set { _damage = value; }
        }

        public Enemy(string name, string type, int health, int damage) : base(name, type, health)
        {
            _damage = damage;   // fixed: was hardcoded to 15, now uses the parameter
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Enemy Name: {Name}");
            Console.WriteLine($"Enemy Type: {Type}");
            Console.WriteLine($"Enemy Health: {Health}");
            Console.WriteLine($"Enemy Damage: {_damage}");
        }
    }

    public class Potion
    {
        private string _potionName;
        private string _effect;

        public string Name
        {
            get { return _potionName; }
            set { _potionName = value; }
        }

        public string PotionEffect
        {
            get { return _effect; }
            set { _effect = value; }
        }

        public Potion(string potionName, string effect)  // fixed: constructor was named 'potion' (lowercase)
        {
            _potionName = potionName;
            _effect = effect;
        }

        public virtual void PotionActive()
        {
            Console.WriteLine($"{_potionName} is now active! {_effect}");
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Potion Name: {_potionName}");
            Console.WriteLine($"Potion Effect: {_effect}");
        }
    }

    public class Haste : Potion     // fixed: was inheriting from 'potion' (lowercase)
    {
        public Haste(string potionName, string effect) : base(potionName, effect)
        {
        }

        public override void PotionActive()    // fixed: override, not virtual; and matches base method
        {
            Console.WriteLine($"{Name} used! {PotionEffect}"); 
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Player wizard = new Player("Gandalf", "Wizard", 150, "Potion of Haste");
            Enemy mage = new Enemy("Merlin", "Mage", 100, 20);
            Potion haste = new Potion("Potion of Haste", "Attacks enemy twice in one turn!");

            Console.WriteLine("-- A duel between the wizard and the mage --");
            Console.WriteLine("________________________________________");
            Console.WriteLine("Press 1 to begin.");
            int choice = Convert.ToInt32(Console.ReadLine());


            //display info
            wizard.DisplayInfo();
            Console.WriteLine("________________________________________");
            mage.DisplayInfo();
            Console.WriteLine("________________________________________");
            Console.WriteLine("PREPARE FOR BATTLE!");

            if (choice == 1)
            {
                while (wizard.Health > 0 && mage.Health > 0)
                {
                    Console.WriteLine("________________________________________");
                    Console.WriteLine("Choose action:");
                    Console.WriteLine("1. Attack");
                    Console.WriteLine("2. Use Potion");
                    Console.WriteLine("3. Defend");
                    Console.Write("Enter your choice: ");
                    int action = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("________________________________________");

                    switch (action)
                    {
                        case 1:
                            wizard.attack();
                            mage.TakeDamage(wizard.AttackDamage); // fixed: pass damage value
                            break;
                        case 2:
                            wizard.UsePotion(haste); 
                            wizard.attack();
                            mage.TakeDamage(wizard.AttackDamage * 2);
                            break;
                        case 3:
                            wizard.Defend();
                            break;
                        default:
                            Console.WriteLine("Invalid choice, turn skipped.");
                            break;
                    }

                    if (mage.Health <= 0)
                    {
                        Console.WriteLine($"{mage.Name} has been defeated! {wizard.Name} wins!");
                        break;
                    }

                    // Enemy turn
                    mage.attack();
                    wizard.TakeDamage(mage.EnemyDamage); // fixed: use Enemy's damage stat
                    if (wizard.Health <= 0)
                    {
                        Console.WriteLine($"{wizard.Name} has been defeated! {mage.Name} wins!");
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Exiting...");
            }
        }
    }
