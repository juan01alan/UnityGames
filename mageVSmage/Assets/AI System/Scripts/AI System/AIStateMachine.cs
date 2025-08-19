using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using jcsilva.CharacterController;

namespace jcsilva.AISystem {

    [RequireComponent(typeof(NavMeshAgent))]
    public class AIStateMachine : MonoBehaviour {

        public Animator Animator;
        // Events Idle Related
        public Action EventAIEnableIdle;
        public Action EventAIDisableIdle;

        // Events Patrol Related
        public Action EventAIEnablePatrol;
        public Action EventAIDisablePatrol;

        // Events Chase Related
        public Action EventAIEnableChase;
        public Action EventAIDisableChase;

        // Events Attack Related
        public Action EventAIEnableAttack;
        public Action EventAIDisableAttack;

        public AudioSource AudioSourceMusic;
        // Events Last Known Location Related
        public Action EventAIEnableLastKnownLocation;
        public Action EventAIDisableLastKnownLocation;
        public float RadiusToCastSpell = 20f;
        [Header("AI Settings")]
        [SerializeField] bool isEnable;
        [SerializeField] AIStates initialState;
        [SerializeField] Transform target;
        [SerializeField] NavMeshAgent agent;

        [Header("AI Behaviours Settings")]
        [SerializeField] float idleDuration;
        [SerializeField] float maxDistanceToView;
        [SerializeField] float minDistanceToAttack;
        [SerializeField] float maxFieldOfView;
        [SerializeField] EnemyController enemyController;
        [SerializeField] bool shouldGetPlayerPosition;

        // AI Patrol Settings
        [HideInInspector]
        [SerializeField] public List<Transform> waypoints = new List<Transform>();

        // Developer Settings
        [HideInInspector]
        [SerializeField] bool enableDebug;
        [HideInInspector]
        [SerializeField] AIStates currentState;
        [HideInInspector]
        AIBehaviour currentBehaviour;
        AIBehaviour[] l_behaviours;
        public bool DebugDistance = false;


        /// <summary>
        /// Dictionary that holds the next state depending on the event occuring,
        /// NOTE: The order is important, so keep the changes you make bellow the comment line
        /// </summary>
        Dictionary<AIEvents, AIStates> nextEvent = new Dictionary<AIEvents, AIStates> {
            [AIEvents.NoLongerIdle] = AIStates.Patrol,
            [AIEvents.SeePlayer] = AIStates.Chase,
            [AIEvents.ReachedDestination] = AIStates.Idle,
            [AIEvents.InRange] = AIStates.Attack,
            [AIEvents.OutOfRange] = AIStates.Chase,
            [AIEvents.LostPlayer] = AIStates.Idle,

            // Add new Events bellow this line
            [AIEvents.ReachedLastKnownPosition] = AIStates.Idle,
        };


        // Start is called before the first frame update
        void Start() {
            if (waypoints.Count > 0) {
                ClearEmptyWaypoints();
            }

            if (agent == null) {
                agent = GetComponent<NavMeshAgent>();
            }

            if (shouldGetPlayerPosition)
            {
                                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
            if (minDistanceToAttack <= 2)
            {
                minDistanceToAttack = 2;
            }
            currentState = initialState;

            InitializeBehaviours();
        }

        private void InitializeBehaviours() {
            l_behaviours = new AIBehaviour[] {
                new IdleBehaviour(this, this),
                new PatrolBehaviour(this, this),
                new ChaseBehaviour(this, this),
                new AttackBehaviour(this, this),
                
                // Add new behaviours bellow this line seperated with a comma ","
                new LastKnownLocation(this, this),
            };

            SelectNextBehaviour(initialState);
        }
        
        /// <summary>
        /// Method that will enables the next Behaviour
        /// </summary>
        /// <param name="nextBehaviour">Next State to which the AI State needs to go</param>
        private void SelectNextBehaviour(AIStates nextBehaviour) {
            currentState = nextBehaviour;
            currentBehaviour = l_behaviours[(int)currentState];
            currentBehaviour.OnBehaviourStart();
        }

        /// <summary>
        /// Method that handles the AI State Change
        /// </summary>
        /// <param name="AIEvent">AI Event</param>
        public void HandleState(AIEvents AIEvent) {

            // Disable current Behaviour
            currentBehaviour.OnBehaviourEnd();

            // Set the next AI State
            AIStates nextState = nextEvent[AIEvent];
            if (Vector3.Distance(transform.position, target.position) <=2)
            {
                nextState = nextEvent[AIEvents.InRange];
            }
            if (DebugDistance)
            {
                Debug.Log("Distance to Player: "+ Vector3.Distance(transform.position, target.position).ToString() );
            }
            AIStates nextBehaviour = nextState;
            if (nextBehaviour == AIStates.LastKnownLocation)
            {
                nextState = AIStates.Idle;
                float distanceToPlayer = Vector3.Distance(transform.position, target.position);
                if (distanceToPlayer <= RadiusToCastSpell)
                {
                    if (AudioSourceMusic != null)
                    {
                        AudioSourceMusic.Play();

                    }
                    enemyController.canShootSpell1 = true;
                    enemyController.canShootSpell2 = true;
                    enemyController.canShootSpell3 = true;
                }
                else
                {
                    if (AudioSourceMusic != null)
                    {
                        AudioSourceMusic.Pause();

                    }
                    enemyController.canShootSpell1 = false;
                    enemyController.canShootSpell2 = false;
                    enemyController.canShootSpell3 = false;
                }
                Animator.SetBool("Attack1", false);
                Animator.SetBool("Attack2", false);
                Animator.SetBool("Attack3", false);
                Animator.SetBool("Walk", false);
            }
            if (nextBehaviour == AIStates.Patrol)
            {
                if (AudioSourceMusic != null)
                {
                    AudioSourceMusic.Pause();

                }
                enemyController.canShootSpell1 = false;
                enemyController.canShootSpell2 = false;
                enemyController.canShootSpell3 = false;
                Animator.SetBool("Attack1", false);
                Animator.SetBool("Attack2", false);
                Animator.SetBool("Attack3", false);
                Animator.SetBool("Walk", true);
            }
            if (nextBehaviour == AIStates.Chase)
            {
                if (AudioSourceMusic != null)
                {
                    AudioSourceMusic.Pause();

                }
                enemyController.canShootSpell1 = false;
                enemyController.canShootSpell2 = false;
                enemyController.canShootSpell3 = false;
                Animator.SetBool("Attack1", false);
                Animator.SetBool("Attack2", false);
                Animator.SetBool("Attack3", false);
                Animator.SetBool("Walk", true);
            }
            if (nextBehaviour == AIStates.Idle)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, target.position);
                if (distanceToPlayer <= RadiusToCastSpell)
                {

                    if (AudioSourceMusic != null)
                    {
                        AudioSourceMusic.Play();

                    }
                    enemyController.canShootSpell1 = true;
                    enemyController.canShootSpell2 = true;
                    enemyController.canShootSpell3 = true;
                }
                else
                {
                    if (AudioSourceMusic != null)
                    {
                        AudioSourceMusic.Pause();

                    }
                    enemyController.canShootSpell1 = false;
                    enemyController.canShootSpell2 = false;
                    enemyController.canShootSpell3 = false;
                }
                    Animator.SetBool("Attack1", false);
                Animator.SetBool("Attack2", false);
                Animator.SetBool("Attack3", false);
                Animator.SetBool("Walk", false);
            }
            if (nextBehaviour == AIStates.Attack)
            {
                agent.SetDestination(transform.position);
                enemyController.canShootSpell1 = false;
                enemyController.canShootSpell2 = false;
                enemyController.canShootSpell3 = false;
                if (enemyController.isSpeller && enemyController.Mana > enemyController.spell1ManaCost)
                {

                    enemyController.canShootSpell1 = true;
                }
                Animator.SetBool("Walk", false);

            }
            // Enable the new AI Behaviour
            SelectNextBehaviour(nextState);
        }

        // Update is called once per frame
        void Update() {
            if (isEnable && currentBehaviour != null)
            {
                currentBehaviour.OnUpdate();
            }
        }

        #region Getters and Setters

        public Transform GetSelfPosition() {
            return this.transform;
        }

        public Transform GetTargetPosition() {
            return target;
        }

        public float GetMaxDistanceToView() {
            return maxDistanceToView;
        }

        public float GetMaxFieldOfView() {
            return maxFieldOfView;
        }

        public float GetIdleDuration() {
            return idleDuration;
        }

        public List<Transform> GetWaypoints() {
            return waypoints;
        }

        public NavMeshAgent GetNavMeshAgent() {
            return agent;
        }

        public float GetMinDistanceToAttack() {
            return minDistanceToAttack;
        }

        public void ClearEmptyWaypoints() {
            waypoints = waypoints.Where(item => item != null).ToList();
        }

        #endregion


    }
}