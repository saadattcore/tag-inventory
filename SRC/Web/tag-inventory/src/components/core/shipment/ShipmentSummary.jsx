import React, { Component } from "react";
import Tabs from "@material-ui/core/Tabs";
import Tab from "@material-ui/core/Tab";
import TabPanel from "@material-ui/lab/TabPanel";
import Box from "@material-ui/core/Box";
import TabContext from "@material-ui/lab/TabContext";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import Pagination from "../../common/reuseable/Pagination";
import config from "../../../config.json";
import Table from "../../common/ui-controls/Table";
import {
  addUpdateShipment,
  getShipment
} from "../../../actions/shipmentActions";
import ShipmentDetail from "../shipment/ShipmentDetail";
import { getReceivedBoxes } from "../../../actions/receivedBoxActions";
import moment from "moment";
import Actions from "../../common/reuseable/Actions";
import { Link } from "react-router-dom";
import ImportScanActions from "../../common/reuseable/ImportScanActions";
import { renderAlert } from "../../common/ui-controls/withAlert";

class ShipmentSummary extends Component {
  constructor(props) {
    super(props);
    this.shipmentID = this.props.match.params.id
      ? this.props.match.params.id
      : -1;

    this.filterSetting = { shipmentID: this.shipmentID };
    this.pageNumber = 1;

    this.currentDate = this.formatDate(new Date());

    this.state = {
      shipment: {
        shipmentID: -1,
        purchaseOrder: "",
        shipmentName: "",
        orderDate: this.currentDate,
        shipmentDate: this.currentDate,
        deliveryDate: this.currentDate,
        status: "",
        statusID: 1
      },
      errors: {
        purchaseOrder: "Purchase order is required",
        shipmentName: "Shipment name is required"
      },
      isFormValid: false,
      selectedTab: 0,
      shipNameTitle: "",
      showAlert: null
    };

    if (this.shipmentID !== -1) {
      delete this.state.errors["purchaseOrder"];
      delete this.state.errors["shipmentName"];
    }
  }

  columns = [
    {
      path: "receivedBoxID",
      title: "ReceivedBoxID",
      key: 1,
      content: id => {
        const navigateUrl = `/received-box/${id}`;
        return <Link to={navigateUrl}>{id}</Link>;
      }
    },
    { path: "startTag", title: "StartFrom", key: 2 },
    { path: "endTag", title: "StartTo", key: 3 },
    { path: "quantity", title: "Quantity", key: 4 },
    { path: "status", title: "Status", key: 5 }
  ];

  pageSize = config.pageConfig.pageSize;

  async componentDidMount() {
    if (this.shipmentID !== -1) {
      try {
        await this.getValidateDate().then(r => {});
      } catch (ex) {}
    }
  }

  handleChange({ target }) {
    let { name, value } = target;
    const state = { ...this.state };

    switch (name) {
      case "tab":
        state.selectedTab = value;
        state.isFormValid =
          value === 0 && Object.keys(this.state.errors).length === 0;
        break;

      default:
        state.shipment[name] = value;
        const error = this.validateProperty(name, value);

        if (error) {
          state.errors[name] = error;
          state.isFormValid = false;
        } else {
          delete state.errors[name];
        }
        if (Object.keys(state.errors).length === 0) {
          state.isFormValid = true;
        }
        break;
    }

    this.setState(state);
  }

  handleFilterElementChange({ value, element, column }) {
    switch (element) {
      case "pagination":
        this.pageNumber = parseInt(value);
        break;
      case "dropdown":
        if (parseInt(value) === -1) delete this.filterSetting["statusID"];
        else this.filterSetting["statusID"] = value;
        this.pageNumber = 1;
        break;
      case "tablefilter":
        if (value) this.filterSetting[column] = value;
        else delete this.filterSetting[column];
        this.pageNumber = 1;
        break;
    }
    const url = this.getURL();
    this.props.getReceivedBoxesAction(url);
  }

  async handleClick(e) {
    e.target.e.preventDefault();
    switch (e.target.action) {
      case "back":
        this.props.history.goBack();
        break;
      case "saveclose":
      case "save":
        const url = `${process.env.REACT_APP_API_URL}shipment/`;
        console.log(e);
        this.props
          .addUpdateShipmentAction(url, this.state.shipment)
          .then(response => {
            if (e.target.action === "saveclose") {
              this.props.history.push("/shipment");
            }

            this.getValidateDate().then(r => {
              const stateClone = { ...this.state };

              stateClone.showAlert = renderAlert.bind(
                null,
                "info",
                "Shipment have been updated sucessfully",
                true,
                e => {
                  stateClone.showAlert = null;
                  this.setState(stateClone);
                }
              );

              this.setState(stateClone);
            });
          })
          .catch(error => {});
        break;
    }
  }

  async getValidateDate() {
    return new Promise((resolve, reject) => {
      const url = `${process.env.REACT_APP_API_URL}shipment/${this.shipmentID}`;
      this.props.getShipmentAction(url).then(response => {
        const cloneState = { ...this.state };
        cloneState.shipment = { ...response };
        cloneState.shipNameTitle = cloneState.shipment.shipmentName;

        cloneState.shipment.shipmentDate = response.shipmentDate
          ? response.shipmentDate
          : null;

        if (!response.deliveryDate) {
          cloneState.shipment.deliveryDate = null;
          cloneState.errors["deliveryDate"] = "Delivery date is required";
          cloneState.isFormValid = false;
        }

        const url = this.getURL();

        this.props
          .getReceivedBoxesAction(url)
          .then(r => {
            this.setState(cloneState);
            resolve(r);
          })
          .catch(ex => {
            reject(ex);
          });
      });
    });
  }

  validateProperty(name, value) {
    if (value && value.toString() === "Invalid Date") return "Invalid Date";

    switch (name) {
      case "purchaseOrder":
        return !value.trim() ? "Purchase order is required" : null;
      case "orderDate":
        return !value ? "Order date is required" : null;
      case "shipmentDate":
        return !value ? "Shipment date is required" : null;
      case "deliveryDate":
        return !value ? "Delivery date is required" : null;
      case "shipmentName":
        return !value.trim() ? "Shipment name is required" : null;
    }
  }

  formatDate(dateObject) {
    if (!dateObject) return null;

    return moment(dateObject).format("MM-DD-YYYY");
  }

  getURL() {
    let baseUrl = `${process.env.REACT_APP_API_URL}received-box/list?pageSize=${this.pageSize}&pageNumber=${this.pageNumber}`;
    let props = Object.keys(this.filterSetting);

    if (props.length > 0) {
      for (let prop in this.filterSetting) {
        baseUrl = baseUrl + `&${prop}=${this.filterSetting[prop]}`;
      }
    }
    console.log(baseUrl);
    return baseUrl;
  }

  render() {
    return (
      <React.Fragment>
        <div className="actions">
          <Actions
            isFormValid={this.state.isFormValid}
            onLinkClick={this.handleClick.bind(this)}
          ></Actions>
        </div>

        <div className="title-summary-wrapper top-buffer">
          <div className="content-title">
            <h3>{this.state.shipNameTitle}</h3>
          </div>

          {this.state.showAlert && this.state.showAlert()}

          <div className="top-buffer">
            <TabContext value={this.state.selectedTab}>
              <Box
                sx={{ borderBottom: 0, borderColor: "divider", width: "25%" }}
              >
                <Tabs
                  value={this.state.selectedTab}
                  variant="fullWidth"
                  indicatorColor="primary"
                  centered
                  onChange={(e, newValue) =>
                    this.handleChange.call(this, {
                      target: {
                        name: "tab",
                        value: newValue
                      }
                    })
                  }
                  aria-label="basic tabs example"
                >
                  <Tab label="Summary" />
                  <Tab label="Received Boxes" />
                </Tabs>
              </Box>
              {this.state.selectedTab === 0 && (
                <TabPanel
                  value={this.state.selectedTab}
                  index={0}
                  style={{ padding: 0 }}
                >
                  <div className="content-area-summary top-buffer">
                    <div className="row">
                      <div className="col-md-5">
                        <ShipmentDetail
                          shipment={this.state.shipment}
                          errors={this.state.errors}
                          readOnly={false}
                          onChangeValue={this.handleChange.bind(this)}
                        ></ShipmentDetail>
                      </div>
                    </div>
                  </div>
                </TabPanel>
              )}
              {this.state.selectedTab === 1 && (
                <TabPanel
                  value={this.state.selectedTab}
                  index={1}
                  style={{ padding: 0 }}
                >
                  <div className="content-area-summary top-buffer">
                    <div className="actions-import-box-no-margin ">
                      <ImportScanActions
                        actionFormType="importBox"
                        onLinkClick={e => {}}
                        navigateURL={`/received-box/import?shipmentID=${this.shipmentID}`}
                      ></ImportScanActions>
                    </div>
                    <div className="row top-buffer">
                      <div className="col-3 select-container">
                        <select
                          className="select-search"
                          onChange={e =>
                            this.handleFilterElementChange({
                              value: e.target.value,
                              element: "dropdown"
                            })
                          }
                        >
                          <option className="select-search" selected value={-1}>
                            All Received Boxes
                          </option>
                          <option className="select-search" value={0}>
                            Imported
                          </option>
                          <option className="select-search" value={1}>
                            Delivery Verified OK
                          </option>
                          <option className="select-search" value={2}>
                            Tags Not In Delivery
                          </option>
                          <option className="select-search" value={3}>
                            Additional Tags In Delivery
                          </option>
                        </select>
                      </div>
                    </div>

                    <div className="row top-buffer">
                      <div className="list-container">
                        {
                          <Table
                            columns={this.columns}
                            data={this.props.receivedBoxes}
                            searchVisible={true}
                            onTableFilter={this.handleFilterElementChange.bind(
                              this
                            )}
                          ></Table>
                        }
                      </div>
                      <div className="row">
                        <Pagination
                          totalRecords={this.props.searchCount}
                          pageSize={this.pageSize}
                          onPageChange={this.handleFilterElementChange.bind(
                            this
                          )}
                          selectedPage={this.pageNumber}
                        ></Pagination>
                      </div>
                    </div>
                  </div>
                </TabPanel>
              )}
            </TabContext>
          </div>
        </div>
      </React.Fragment>
    );
  }
}

ShipmentSummary.propTypes = {
  shipment: PropTypes.object.isRequired,
  addUpdateShipment: PropTypes.func.isRequired,
  getShipment: PropTypes.func.isRequired
};

const mapStateToProps = state => {
  return {
    shipment: state.shipment.shipment,
    receivedBoxes: state.receivedBox.receivedBoxes,
    totalCount: state.receivedBox.totalCount,
    searchCount: state.receivedBox.searchCount
  };
};

const mapDispatchToProps = disptach => ({
  addUpdateShipmentAction: (url, shipment) =>
    disptach(addUpdateShipment(url, shipment)),
  getShipmentAction: url => disptach(getShipment(url)),
  getReceivedBoxesAction: url => disptach(getReceivedBoxes(url))
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(ShipmentSummary);
