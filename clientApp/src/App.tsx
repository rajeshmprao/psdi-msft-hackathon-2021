import { Text } from "@fluentui/react";
import React from "react";
import Header from "./features/header/header";
import CustomerMetrics from "./features/metrics/customerMetrics";
import PortfolioMetrics from "./features/metrics/portfolioMetrics";

function App() {
  return (
    <>
      <Header />
      <div
        style={{
          display: "flex",
          flexDirection: "column",
          justifyContent: "flex-start",
          flexWrap: "wrap",
          gap: "15px 0px",
          padding: "20px",
        }}
      >
        <Text
          key="portfolioHealth"
          variant="xxLarge"
          style={{ textAlign: "center" }}
        >
          Portfolio Health - Green
        </Text>
        <PortfolioMetrics />
        <CustomerMetrics />
      </div>
    </>
  );
}

export default App;
