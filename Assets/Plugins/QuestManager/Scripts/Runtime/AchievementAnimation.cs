using UnityEngine;

namespace GameLabGraz.QuestManager
{
    public class AchievementAnimation : MonoBehaviour
    {
        public float rotationSpeed = 60f;
        public float jumpHeight = 2f;
        public float jumpDuration = 1f;
        public float timeBetweenJumps = 2f;

        private float jumpStartTime;
        private bool isJumping = false;

        void Update()
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

            if (Time.time - jumpStartTime >= timeBetweenJumps && !isJumping)
            {
                isJumping = true;
                jumpStartTime = Time.time;
                Jump();
            }
        }

        void Jump()
        {
            Vector3 initialPosition = transform.position;
            Vector3 targetPosition = transform.position + Vector3.up * jumpHeight;

            StartCoroutine(JumpAnimation(initialPosition, targetPosition));
        }

        System.Collections.IEnumerator JumpAnimation(Vector3 start, Vector3 target)
        {
            float elapsedTime = 0f;

            while (elapsedTime < jumpDuration)
            {
                float t = elapsedTime / jumpDuration;
                transform.position = Vector3.Lerp(start, target, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = target;

            isJumping = false;
        }
    }
}
