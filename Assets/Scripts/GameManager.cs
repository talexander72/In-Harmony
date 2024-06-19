using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public ParticleSystem explosion;
    public float respawnTime = 3.0f;
    public int lives = 3;
    public float respawnInvulnerabilityTime = 3.0f;
    public int score = 0;
    private bool gameOver = false;

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        explosion.transform.position = asteroid.transform.position;
        explosion.Play();

        if (asteroid.size < 0.75f) {
            score += 100;
        } else if (asteroid.size < 1.15f) {
            score += 50;
        } else {
            score += 25;
        }
    }

    public void PlayerDied()
    {
        explosion.transform.position = player.transform.position;
        explosion.Play();

        lives--;

        if (lives <= 0) {
            GameOver();
        } else {
            Invoke(nameof(Respawn), respawnTime);
        } 
    }

    private void Respawn()
    {
        player.transform.position = Vector3.zero;
        player.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
        player.isAlive = true;
        
        Invoke(nameof(TurnOnCollisions), respawnInvulnerabilityTime);
    }

    private void TurnOnCollisions()
    {
        player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void GameOver()
    {
        gameOver = true;
        lives = 3;
        score = 0;

        FindObjectOfType<AsteroidSpawner>().StopSpawning();
        Invoke(nameof(Respawn), respawnTime);
    }
}
