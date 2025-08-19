using UnityEngine;

namespace Basic.PublicVariables
{
    public class PublicFloat : MonoBehaviour
    {
        [SerializeField] private float _float;

        public float FloatGetSet { get => _float; set => _float = value; }

        public void PrintFloat()
        {
            print(_float);
        }

    }

}