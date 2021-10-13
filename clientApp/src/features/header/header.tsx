import React, { useEffect, useState } from "react";
import { CoherenceHeader } from "@cseo/controls";
const Header: React.FunctionComponent = () => {
  return (
    <CoherenceHeader
      headerLabel="header"
      appNameSettings={{
        label: "PSDI Portal",
      }}
      farRightSettings={{
        profileSettings: {
          fullName: "John Doe",
          emailAddress: "jd@microsoft.com",
          imageUrl: undefined,
          logOutLink: "/Home/Signout",
          onLogOut: () => {
            alert("Handled the logout");
          },
        },
      }}
    />
  );
};

export default Header;
