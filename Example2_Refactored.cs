using System;
using UnityEngine;
using Example1;

namespace Example2
{
    public interface IHealth
    {
        int Health { get; }

        event Action<int> HealthChanged;
    }

    public class ExtendedPlayer : IHealth
    {
        public int Health => player.Health;
        public event Action<int> HealthChanged;
        private Player player;
        
        public ExtendedPlayer(Player player)
        {
            this.player = player;
        }

        public void Hit(int damage)
        {
            player.Hit(damage);
            HealthChanged?.Invoke(player.Health);
        }
    }

    public class HealthView : IDisposable
    {
        private TextView textView = new TextView();
        private int? currentHealth;
        private IHealth healthObject;

        public HealthView(IHealth healthObject)
        {
            this.healthObject = healthObject;
            this.healthObject.HealthChanged += OnHealthChanged;
            OnHealthChanged(this.healthObject.Health);
        }

        public void Dispose()
        {
            healthObject.HealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(int newHealth)
        {
            textView.Text = newHealth.ToString();

            if (currentHealth != null && newHealth - currentHealth < -10)
            {
                textView.Color = Color.red;
            }
            else
            {
                textView.Color = Color.white;
            }
            currentHealth = newHealth;
        }
    }

    class Program
    {
        private const int DefaultHealth = 100;
        private const int DefaultDamage = 10;

        public static void Main(string[] args)
        {
            var player = new Player(DefaultHealth);
            var settings = new Settings(DefaultDamage);
            
            var playerDecorator = new ExtendedPlayer(player);
            var healthView = new HealthView(playerDecorator);

            playerDecorator.Hit(settings.Damage);
        }
    }
}
