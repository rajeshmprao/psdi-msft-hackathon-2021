import { Text, IconButton, IIconProps } from "@fluentui/react";
import React, { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../../app/hooks";
import MetricsCustomization from "./MetricsCustomization";
import {
  getAllMetrics,
  getPortfolioMetrics,
  selectPortfolioMetrics,
  updateMetricCustomizationPanel,
} from "./metricsSlice";
import PortfolioMetricsCard from "./portfolioMetricsCard";

function PortfolioMetrics() {
  const dispatch = useAppDispatch();
  const portfolioMetrics = useAppSelector(selectPortfolioMetrics);
  useEffect(() => {
    dispatch(getPortfolioMetrics());
    dispatch(getAllMetrics());
  }, []);
  const addIcon: IIconProps = { iconName: "Add" };

  return (
    <>
      <MetricsCustomization />
      <Text
        key="portfolioHealth"
        variant="xLarge"
        style={{ textAlign: "center" }}
      >
        Portfolio Metrics
      </Text>
      <div
        style={{
          display: "flex",
          flexDirection: "row",
          flexWrap: "wrap",
          justifyContent: "center",
          gap: "20px",
        }}
      >
        {portfolioMetrics.map((metrics) => (
          <PortfolioMetricsCard props={metrics} />
        ))}
        <IconButton
          iconProps={addIcon}
          style={{ alignSelf: "center" }}
          title="Add"
          ariaLabel="Add"
          onClick={() => {
            dispatch(updateMetricCustomizationPanel({ isOpen: true }));
          }}
        />
      </div>
    </>
  );
}

export default PortfolioMetrics;
