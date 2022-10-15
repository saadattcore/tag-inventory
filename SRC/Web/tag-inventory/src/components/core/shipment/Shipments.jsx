import React from "react";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import Menu from "@material-ui/core/Menu";
import {
  getShipments,
  downloadExportPackage
} from "../../../actions/shipmentActions";
import http from "../../../services/HttpModule";
import Table from "../../common/ui-controls/Table";
import Pagination from "../../common/reuseable/Pagination";
import ShipmentActions from "../../common/reuseable/ShipmentActions";
import DialogComponent from "../../common/ui-controls/DialogComponent";
import { Link } from "react-router-dom";
import { Select, FormControl, MenuItem, InputLabel } from "@material-ui/core";
import { Component } from "react";
import config from "../../../config.json";
import { renderAlert } from "../../common/ui-controls/withAlert";

class Shipments extends Component {
  constructor(props) {
    super(props);
    this.columns = [
      {
        path: "checkBox",
        title: "Select",
        key: 6,
        content: arg => <input type="checkbox"></input>
      },

      {
        path: "shipmentID",
        title: "Shipment ID",
        key: 1,
        content: arg => <Link to={`/shipment/summary/${arg}`}>{arg}</Link>
      },
      { path: "purchaseOrder", title: "Purchase Order", key: 2 },
      { path: "shipmentName", title: "Shipment", key: 2 },
      { path: "orderDate", title: "Order Date", key: 3, type: "calender" },
      {
        path: "shipmentDate",
        title: "Shipment Date",
        key: 4,
        type: "calender"
      },
      { path: "status", title: "Status", key: 5 },
      {
        path: "",
        title: "Action",
        key: 5,
        content: (arg, shipment) => {
          return (
            <React.Fragment>
              <a
                href="#"
                onClick={e =>
                  this.handleClick.call(this, {
                    action: "toggleContextMenu",
                    name: "btnContextMenu",
                    shipment: shipment,
                    element: e.target
                  })
                }
              >
                <i className="ms-Icon ms-font-xl ms-Icon--MoreVertical"></i>
              </a>
              {shipment.openContextMenu && (
                <div>
                  <Menu
                    id="long-menu"
                    MenuListProps={{
                      "aria-labelledby": "long-button"
                    }}
                    anchorEl={shipment.anchorEl}
                    open={shipment.openContextMenu}
                    onClose={e =>
                      this.handleMenuClose.call(this, {
                        action: "generate export package",
                        element: e,
                        shipment: shipment
                      })
                    }
                    PaperProps={{
                      style: {
                        maxHeight: 48 * 4.5,
                        width: "28ch"
                      }
                    }}
                  >
                    {this.contextMenuOptions.map(option => (
                      <MenuItem
                        key={option}
                        //selected={option === "Pyxis"}
                        onClick={e =>
                          this.handleMenuClose.call(this, {
                            action: "generate export package",
                            element: e,
                            shipment: shipment
                          })
                        }
                      >
                        {option}
                      </MenuItem>
                    ))}
                  </Menu>
                </div>
              )}

              {shipment.openDialog && (
                <DialogComponent
                  title={"Confirm"}
                  open={shipment.openDialog}
                  content={"Shipment contains free tags?"}
                  actions={[
                    {
                      action: true,
                      type: "contextmenu",
                      text: "Yes"
                    },
                    {
                      action: false,
                      type: "contextmenu",
                      text: "No"
                    }
                  ]}
                  onDialogClose={e => {
                    console.log(e.target);
                    if (!e.target.action) return;
                    let url = `${process.env.REACT_APP_API_URL}shipment/create-shipment-package?shipmentID=${shipment.shipmentID}&containsFreeTags=${e.target.action}`;

                    this.props
                      .downloadExportPackageAction(url, shipment.shipmentID)
                      .then(r => {})
                      .catch(ex => {
                        console.log(ex);

                        this.displayAlert(ex.response.statusText, "error");
                      });

                    const stateClone = { ...this.state };
                    const t = stateClone.shipments.find(
                      t => t.shipmentID === shipment.shipmentID
                    );

                    t.openDialog = false;
                    this.setState(stateClone);
                  }}
                ></DialogComponent>
              )}
            </React.Fragment>
          );
        }
      }
    ];

    this.state = {
      shipments: [],
      totalRecords: 0,
      showAlert: null
    };

    this.filterSetting = {};
    this.pageConfig = {
      pageSize: config.pageConfig.pageSize,
      pageNumber: config.pageConfig.pageNumber
    };

    this.contextMenuOptions = ["Generate Export Package", "Delete"];
  }

  getURL(arg) {
    const { pageSize, pageNumber } = this.pageConfig;
    let baseUrl = `${process.env.REACT_APP_API_URL}shipment/list?pageSize=${pageSize}&pageNumber=${pageNumber}`;
    let props = Object.keys(this.filterSetting);

    if (props.length > 0) {
      for (let prop in this.filterSetting) {
        baseUrl = baseUrl + `&${prop}=${this.filterSetting[prop]}`;
      }
    }
    console.log(baseUrl);
    return baseUrl;
  }

  componentDidMount() {
    const url = this.getURL();
    console.log(url);
    this.props
      .getShipmentAction(url)
      .then(r => {
        const stateClone = { ...this.state };
        stateClone.shipments = this.props.shipments;
        this.setState(stateClone);
      })
      .catch(e => {});
  }

  handleChange = ({ value, element, column, type, shipment }) => {
    const stateClone = { ...this.state };

    switch (element) {
      case "pagination":
        console.log(this);
        this.pageConfig.pageNumber = parseInt(value);
        break;
      case "dropdown":
        if (parseInt(value) === 0) delete this.filterSetting["statusID"];
        else this.filterSetting["statusID"] = value;
        this.pageConfig.pageNumber = 1;
        break;
      case "tablefilter":
        if (type === "calender") {
          if (value && isNaN(Date.parse(value))) {
            console.log("invalid date");
            return;
          } else {
            const year = new Date(value).getFullYear();
            if (year < 1753 || year > 9999) {
              alert("Date is out of range");
              return;
            }
          }
        }

        if (value) this.filterSetting[column] = value;
        else delete this.filterSetting[column];
        this.pageConfig.pageNumber = 1;
        break;
    }
    const url = this.getURL();
    this.props.getShipmentAction(url);
  };

  handleMenuClose({ action, element, shipment }) {
    const stateClone = { ...this.state };

    switch (action.toLowerCase()) {
      case "generate export package":
        const stateShipment = stateClone.shipments.find(
          s => (s.shipmentID = shipment.shipmentID)
        );

        stateShipment.anchorEl = null;
        stateShipment.openContextMenu = Boolean(stateShipment.anchorEl);
        stateShipment.openDialog = true;
        this.setState(stateClone);
        break;
    }
  }

  handleClick({ action, name, shipment, element }) {
    const stateClone = { ...this.state };
    switch (action) {
      case "toggleContextMenu":
        const stateShipment = stateClone.shipments.find(
          s => s.shipmentID === shipment.shipmentID
        );

        stateShipment.openContextMenu = Boolean(element);
        stateShipment.anchorEl = element;

        this.setState(stateClone);
        break;
    }
  }

  handleDialogClose(arg) {}

  displayAlert(content, type) {
    return new Promise((resolve, reject) => {
      const stateClone = { ...this.state };
      stateClone.showAlert = renderAlert.bind(null, type, content, true, e => {
        const stateClone = { ...this.state };
        stateClone.showAlert = null;
        this.setState(stateClone);
        resolve("fulfilled");
      });

      this.setState(stateClone);
    });
  }

  render() {
    return (
      <React.Fragment>
        <div className="actions">
          <ShipmentActions></ShipmentActions>
        </div>
        {this.state.showAlert && this.state.showAlert()}
        <div className="content-area-main">
          <div className="row">
            <div className="col-3 select-container">
              <select
                className="select-search"
                onChange={e =>
                  this.handleChange({
                    value: e.target.value,
                    element: "dropdown"
                  })
                }
              >
                <option className="select-search" selected value={0}>
                  All Shipment
                </option>
                <option className="select-search" value={1}>
                  Ordered Shipments
                </option>
                <option className="select-search" value={2}>
                  Delivered Shipments
                </option>
              </select>
            </div>
          </div>

          <div className="row top-buffer">
            <div className="list-container">
              {
                <Table
                  columns={this.columns}
                  data={this.props.shipments}
                  onTableFilter={this.handleChange}
                  searchVisible={true}
                ></Table>
              }
            </div>
            <div className="row">
              <Pagination
                totalRecords={this.props.searchCount}
                pageSize={this.pageConfig.pageSize}
                onPageChange={this.handleChange}
                selectedPage={this.pageConfig.pageNumber}
              ></Pagination>
            </div>
          </div>
        </div>
      </React.Fragment>
    );
  }
}

Shipments.propTypes = {
  shipment: PropTypes.array.isRequired,
  getShipments: PropTypes.func.isRequired,
  totalCount: PropTypes.number.isRequired,
  searchCount: PropTypes.number.isRequired
};

const mapStateToProps = state => {
  return {
    shipments: state.shipment.shipments,
    totalCount: state.shipment.totalCount,
    searchCount: state.shipment.searchCount
  };
};

const mapDispatchToProps = dispatch => ({
  getShipmentAction: url => dispatch(getShipments(url)),
  downloadExportPackageAction: (url, shipmentID) =>
    dispatch(downloadExportPackage(url, shipmentID))
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(Shipments);
