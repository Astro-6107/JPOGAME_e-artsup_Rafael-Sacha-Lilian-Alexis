using UnityEngine;

namespace JPO
{
    public class ExitKey : Item
    {
        protected override void OnCollected(Collider2D collision)
        {
            base.OnCollected(collision);
            // Invoke the event KeyCollected to open the exit of the level
            EventBus.OnKeyCollected();
        }
    }
}