import { MetricInterface } from "./metricsSlice";
import { SharedColors } from '@fluentui/theme';


export const getMetricColor = (props: MetricInterface): string => {
  if (props.value) {
    if (parseFloat(props.value) > parseFloat(props.upperThreshold))
      return SharedColors.green10;
    if (parseFloat(props.value) < parseFloat(props.lowerThreshold))
      return SharedColors.red10;
  }
  return SharedColors.yellow10;
};
