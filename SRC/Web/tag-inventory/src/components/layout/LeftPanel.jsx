import React from "react";
import { Link } from "react-router-dom";
import { fontWeight } from "@material-ui/system";

const LeftPanel = (props) => {
  console.log(props);
  return (
    <React.Fragment>
      <ul>
        <li>
          <Link
            to="/"
            onClick={() => {
              props.onMenuClick("Home");
            }}
          >
            <div
              className={
                props.selectedMenu === "Home" ? "selected-left-menu" : null
              }
            >
              <span title="Edit" className="left-margin">
                {/*  <i style={{ fontFamily: "FabricMDL2Icons" }} aria-hidden="true">
                  
                </i> */}
                <i
                  aria-hidden="true"
                  className="ms-Icon ms-font-xl ms-Icon--Home i-size"
                ></i>
              </span>
              <span
                className={
                  props.selectedMenu === "Home" ? " selected-menu-text" : ""
                }
              >
                Home
              </span>
            </div>
          </Link>
        </li>
        <li>
          <Link
            to="/shipment"
            onClick={() => {
              props.onMenuClick("Shipment");
            }}
          >
            <div
              className={
                props.selectedMenu === "Shipment" ? "selected-left-menu" : null
              }
            >
              <span title="Edit" className="left-margin">
                <i
                  aria-hidden="true"
                  className="ms-Icon ms-font-xl ms-Icon--DeliveryTruck i-size"
                ></i>
              </span>
              <span
                className={
                  props.selectedMenu === "Shipment" ? " selected-menu-text" : ""
                }
              >
                Shipment
              </span>
            </div>
          </Link>
        </li>

        <li>
          <Link
            to="/received-box"
            onClick={() => {
              props.onMenuClick("Received Box");
            }}
          >
            <div
              className={
                props.selectedMenu === "Received Box"
                  ? "selected-left-menu"
                  : null
              }
            >
              <span title="Edit" className="left-margin">
                <i
                  aria-hidden="true"
                  className="ms-Icon ms-font-xl ms-Icon--Packages i-size"
                ></i>
              </span>
              <span
                className={
                  props.selectedMenu === "Received Box"
                    ? "selected-menu-text"
                    : ""
                }
              >
                Received Box
              </span>
            </div>
          </Link>
        </li>

        <li>
          <Link
            to="/issued-box"
            onClick={() => {
              props.onMenuClick("Issued Box");
            }}
          >
            <div
              className={
                props.selectedMenu === "Issued Box"
                  ? "selected-left-menu"
                  : null
              }
            >
              <span title="Edit" className="left-margin">
                <i
                  aria-hidden="true"
                  className="ms-Icon ms-font-xl ms-Icon--Product i-size"
                ></i>
              </span>
              <span
                className={
                  props.selectedMenu === "Issued Box"
                    ? "selected-menu-text"
                    : ""
                }
              >
                Issued Box
              </span>
            </div>
          </Link>
        </li>

        <li>
          <Link
            to="/tag"
            onClick={() => {
              props.onMenuClick("Tag");
            }}
          >
            <div
              className={
                props.selectedMenu === "Tag" ? "selected-left-menu" : null
              }
            >
              <span title="Edit" className="left-margin">
                <i
                  aria-hidden="true"
                  className="ms-Icon ms-font-xl ms-Icon--Tag i-size"
                ></i>
              </span>
              <span
                className={
                  props.selectedMenu === "Tag" ? "selected-menu-text" : ""
                }
              >
                Tag
              </span>
            </div>
          </Link>
        </li>

        <li>
          <Link
            to="#"
            onClick={() => {
              props.onMenuClick("Reports");
            }}
          >
            <div
              className={
                props.selectedMenu === "Reports" ? "selected-left-menu" : null
              }
            >
              <span title="Edit" className="left-margin">
                <i
                  aria-hidden="true"
                  className="ms-Icon ms-font-xl ms-Icon--AnalyticsReport i-size"
                ></i>
              </span>
              <span
                className={
                  props.selectedMenu === "Reports" ? "selected-menu-text" : ""
                }
              >
                Reports
              </span>
            </div>
          </Link>
        </li>
      </ul>
    </React.Fragment>
  );
};

export default LeftPanel;
