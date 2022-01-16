using System;
using UnityEngine;

namespace GameLabGraz.LimeSurvey.Data
{
    [Serializable]
    public class SubQuestion : BaseQuestion
    {
        [SerializeField] private int scale_id;

        public int ScaleId => scale_id;
    }
}
