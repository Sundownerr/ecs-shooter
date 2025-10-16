using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems
{
    public class Flying3dGrid : MonoBehaviour
    {
        public Vector3 StartPosition;
        public Vector3 EndPosition;
        public int Step = 1;
        private readonly List<Vector3> _positions = new();

        private void Start() =>
            ScanPositions();

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(StartPosition, 0.5f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(EndPosition, 0.5f);

            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(StartPosition, EndPosition);

            if (_positions.Count > 0) {
                var color = Color.yellow - new Color(0, 0, 0, 0.6f);
                Gizmos.color = color;
                foreach (var position in _positions)
                    Gizmos.DrawSphere(position, Step *0.5f);
            }
        }

        private void ScanPositions()
        {
            var position = StartPosition;
            var endPosition = EndPosition;

            var x = position.x;
            var y = position.y;
            var z = position.z;

            var endX = endPosition.x;
            var endY = endPosition.y;
            var endZ = endPosition.z;

            for(var i = x; i < endX; i += Step) {
                for(var j = y; j < endY; j += Step) {
                    for(var k = z; k < endZ; k += Step) {
                        position.x = i;
                        position.y = j;
                        position.z = k;

                        if (!Physics.CheckSphere(position, Step))
                            _positions.Add(position);
                    }
                }
            }
        }
    }
}