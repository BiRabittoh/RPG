using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class IsEnemyTrigger : MonoBehaviour
{
    private GameMaster gm;
    public string EnemyType;

    private void Start()
    {
        gm = GameMaster.Instance;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            gm.EnterBattle(transform.parent.name, EnemyType);
        }
            
    }
}
