using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Example1
{
    [Serializable]
    public struct PlayerData
    {
        public int Health { get; set; }
    }

    [Serializable]
    public struct SettingsData
    {
        public int Damage { get; set; }
    }

    public class Settings
    {
        public int Damage { get; }

        public Settings(int damage)
        {
            Damage = damage;
        }
    }

    public class Player
    {
        public int Health { get; private set; }

        public Player(int health)
        {
            Health = health;
        }

        public void Hit(int damage)
        {
            if (damage <= 0)
            {
                throw new ArgumentException("Damage must be a positive value.", nameof(damage));
            }

            Health -= damage;
        }
    }

    class Program
    {
        private const string NewPlayerPath = "NewPlayer.json";
        private const string SettingsPath = "Settings.json";
        private const int DefaultHealth = 100;
        private const int DefaultDamage = 10;

        public static void Main(string[] args)
        {
            var health = TryLoadFromFile(NewPlayerPath, out PlayerData playerData) ? playerData.Health : DefaultHealth;
            var player = new Player(health);

            var damage = TryLoadFromFile(SettingsPath, out SettingsData settingsData) ? settingsData.Damage : DefaultDamage;
            var settings = new Settings(damage);

            player.Hit(settings.Damage);
        }

        public static bool TryLoadFromFile<T>(string filePath, out T result)
        {
            // This is where the data from the file is deserialized
            throw new NotImplementedException();
        }
    } 
}
