using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoolModel
{
    public class StoolParameters
    {

        private Dictionary<ParameterType, Parameter> Parameters { get; set; }

        public Dictionary<ParameterType, string> Errors { get; set; }

        public StoolParameters()
        {
            Parameters = new Dictionary<ParameterType, Parameter>()
            {
                    { ParameterType.SeatWidth,
                        new Parameter(300, 350, 400, ParameterType.SeatWidth)},
                    { ParameterType.SeatHeight,
                        new Parameter(10, 30, 50, ParameterType.SeatHeight)},
                    { ParameterType.LegsWidth,
                        new Parameter(20, 40, 60, ParameterType.LegsWidth)},
                    { ParameterType.LegsHeight,
                        new Parameter(300, 400, 500, ParameterType.LegsHeight)},
                    { ParameterType.LegSpacing,
                        new Parameter(190, 210, 230, ParameterType.LegSpacing)}
                };
        }
    }
}
