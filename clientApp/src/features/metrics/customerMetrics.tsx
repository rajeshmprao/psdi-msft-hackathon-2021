import { Text } from "@fluentui/react";
import React, { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../../app/hooks";
import CustomerMetricsCard from "./customerMetricsCard";
import { getCustomerMetrics, selectCustomerMetrics } from "./metricsSlice";

function CustomerMetrics() {
  const dispatch = useAppDispatch();
  const customerMetrics = useAppSelector(selectCustomerMetrics);
  useEffect(() => {
    dispatch(getCustomerMetrics());
  }, []);

  return (
    <>
      <Text
        key="customerHealth"
        variant="xLarge"
        style={{ textAlign: "center" }}
      >
        Customer Metrics
      </Text>
      <div
        style={{
          display: "flex",
          flexDirection: "row",
          justifyContent: "center",
          gap: "20px",
          flexWrap: "wrap",
        }}
      >
        {customerMetrics.map((metrics) => (
          <CustomerMetricsCard metrics={metrics} />
        ))}
      </div>
    </>
  );
}

export default CustomerMetrics;
