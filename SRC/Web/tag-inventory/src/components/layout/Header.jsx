import React from "react";

const Header = ({ link }) => {
  return (
    <React.Fragment>
      <div className="col-2 header-containers-height">
        <h4 className="header-project-lable">Tag Inventory</h4>
      </div>
      <div className="col-1 header-containers-height">
        <p className="display-inline">{link}</p>
      </div>
    </React.Fragment>
  );
};

export default Header;
