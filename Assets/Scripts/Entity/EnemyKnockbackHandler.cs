using System.Collections;

using UnityEngine;
using UnityEngine.AI;

using Utilities;

namespace Entity {
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyKnockbackHandler : KnockbackHandler {
        [SerializeField] private NavMeshAgent agent;

        protected override void Start() {
            base.Start();
            agent = GetComponent<NavMeshAgent>();
        }

        public override void Knockback(float damage, KnockbackData data) {
            if (data.applyKnockback) {
                agent.updatePosition = false;
                rb2D.AddForce((rb2D.position - data.pos).normalized * damage, ForceMode2D.Impulse);
                StartCoroutine(ResetAgent());
            }
        }

        private IEnumerator ResetAgent() {
            yield return Yielders.WaitForSeconds(delay);
            rb2D.velocity = Vector2.zero;
            if (agent) { //Support for stationary enemies that can be moved - probably just shouldn't be on them...?
                if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) {
                    while (((Vector2) transform.position - (Vector2) hit.position).sqrMagnitude >= 0.01f) {
                        rb2D.MovePosition(Vector2.MoveTowards(rb2D.position, hit.position, Time.fixedDeltaTime));
                        yield return Yielders.waitForFixedUpdate;
                    }
                    agent.Warp((Vector2) hit.position);
                } else {
                    agent.Warp(rb2D.position); //No! Bad snapping!
                }
                yield return Yielders.waitForFixedUpdate;
                agent.updatePosition = true;
            }
        }
    }
}