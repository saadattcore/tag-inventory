import React, { Component } from "react";
import Tabs from "@material-ui/core/Tabs";
import Tab from "@material-ui/core/Tab";
import TabPanel from "@material-ui/lab/TabPanel";
import Box from "@material-ui/core/Box";
import TabContext from "@material-ui/lab/TabContext";
import IssuedBoxItem from "./IssuedBoxItem";
import IssuedBoxTags from "./IssuedBoxTags";
import IssuedBoxScanTags from "./IssuedBoxScanTags";
import IssuedBoxTimeLine from "./IssuedBoxTimeLine";
import { getIssuedBoxTimeLine } from "../../../actions/issuedBoxActions";
import IssuedBoxDetailActions from "../../common/reuseable/IssuedBoxDetailActions";
import {
  getIssuedBox,
  updateIssuedBoxesStatus,
  createUpdateIssueBox,
  downloadSerialList,
  updateIssuedBox
} from "../../../actions/issuedBoxActions";

import { getDistributors } from "../../../actions/lookupActions";

import { connect } from "react-redux";
import { getShipment } from "../../../actions/shipmentActions";
import { Link } from "react-router-dom";
import Actions from "../../common/reuseable/Actions";
import { renderAlert } from "../../common/ui-controls/withAlert";

class IssuedBoxSummary extends Component {
  constructor(props) {
    super(props);
    this.issuedBoxID = this.props.match.params.issuedBox;
  }

  columns = [
    {
      path: "tagID",
      title: "Tag ID",
      key: 1,
      content: id => {
        const navigateUrl = `/tag/summary/${id}`;
        return <Link to={navigateUrl}>{id}</Link>;
      }
    },
    { path: "tagNumber", title: "Tag Number", key: 2 },
    { path: "serialNumber", title: "Serial Number", key: 3 },
    { path: "status", title: "Tag Status", key: 4 },
    { path: "tagType", title: "Tag Type", key: 5 }
  ];

  state = {
    selectedTab: 0,
    statusID: 0,
    distributorID: 0,
    issuedBoxClone: { tags: [] },
    issuedBoxUpd: {},
    activeForm: "list",
    scanTags: [],
    nextStatus: { name: "", id: -1 },
    isFormValid: false,
    showAlert: null
  };

  async componentDidMount() {
    await this.getData();
  }

  getData() {
    this.getIssuedBox().then(r => {
      this.getBoxTimeLine().then(r => {
        let baseUrl = `${process.env.REACT_APP_API_URL}lookup/get-distributors`;
        this.props.getDistributorsAction(baseUrl);
      });
    });
  }

  handleChange({ name, value }) {
    const cloneState = { ...this.state };
    switch (name) {
      case "tab":
        console.log(value);
        cloneState.selectedTab = value;
        break;
      default:
        cloneState.issuedBoxClone[name] = value;
        cloneState.isFormValid = true;
        if (name === "statusID" || name === "distributorID") {
          cloneState[name] = value;
        }
        break;
    }

    this.setState(cloneState);
  }

  async handleLinkClick(e) {
    e.target.e.preventDefault();
    switch (e.target.action) {
      case "assigntags":
        this.props.history.push(`/issued-box/create/${this.issuedBoxID}`);
        break;

      case "completebox":
        this.updateIssuedBoxStatus(this.state.nextStatus.id).then(r => {
          this.getIssuedBox().then(res => {
            const state = { ...this.state };

            state.showAlert = renderAlert.bind(
              null,
              "info",
              "Issued box status updated",
              true,
              e => {
                state.showAlert = null;
                this.setState(state);
              }
            );

            const nextState = this.setNextStatus(this.state.nextStatus.id);
            state.nextStatus = { ...nextState };
            this.setState(state);
          });
        });
        break;
      case "back":
        this.props.history.goBack();
        break;

      case "print":
        let baseUrl = `${
          process.env.REACT_APP_API_URL
        }issued-box/serial-list?issuedBoxIDList=${[this.issuedBoxID]}`;
        console.log(baseUrl);
        this.props.downloadSerialListAction(baseUrl).then(response => {});
        break;

      case "saveclose":
      case "save":
        const url = `${process.env.REACT_APP_API_URL}issued-box/update-box`;
        this.props
          .updateIssuedBoxAction(url, {
            ...this.state.issuedBoxClone
          })
          .then(r => {
            if (e.target.action === "saveclose") {
              this.props.history.push("/issued-box");
            } else {
              const stateClone = { ...this.state };

              stateClone.showAlert = renderAlert.bind(
                null,
                "info",
                "Issued box updated sucessfully",
                true,
                e => {
                  stateClone.showAlert = null;
                  this.setState(stateClone);
                }
              );

              this.setState(stateClone);
            }
            this.getData();
          });
        break;

      default:
        console.log("unknown action");
        break;
    }
  }

  handleClick(arg) {
    const cloneState = { ...this.state };
    arg.target.e.preventDefault();

    console.log(arg);

    switch (arg.target.action) {
      case "scan":
        cloneState.activeForm = "scantags";
        this.setState(cloneState);
        break;
      case "saveclose":
      case "save":
        if (cloneState.activeForm === "scantags") {
          cloneState.activeForm = "list";
          this.setState(cloneState);
        } else {
          const url = `${process.env.REACT_APP_API_URL}issued-box`;
          this.saveTags(url).then(r => {
            if (arg.target.action === "saveclose") {
              this.props.history.push(`/issued-box`);
            }
          });
        }

        break;

      case "assign":
        break;

      case "back":
        if (this.state.activeForm === "scantags") {
          cloneState.activeForm = "list";
          this.setState(cloneState);
        } else {
          this.props.history.goBack();
        }
        break;
    }
  }

  saveTags(url) {
    return new Promise((resolve, reject) => {
      this.props
        .createUpdateIssueBoxAction(url, { ...this.state.issuedBoxClone })
        .then(r => {
          resolve(true);
        })
        .catch(ex => reject(ex));
    });
  }

  getIssuedBox() {
    return new Promise((resolve, reject) => {
      const url =
        `${process.env.REACT_APP_API_URL}issued-box/` + this.issuedBoxID;
      this.props.getIssuedBoxAction(url).then(response => {
        const stateClone = { ...this.state };
        console.log(response);
        stateClone.issuedBoxClone = { ...response };
        stateClone.statusID = response.statusID;
        stateClone.distributorID = response.distributorID;
        const newStatus = this.setNextStatus(response.statusID);
        stateClone.nextStatus = { ...newStatus };
        this.props
          .getShipmentAction(
            `${process.env.REACT_APP_API_URL}shipment/` + response.shipmentID
          )
          .then(r => {
            this.setState(stateClone);
            resolve(true);
          });
      });
    });
  }

  updateIssuedBoxStatus(statusID) {
    return new Promise((resolve, reject) => {
      const url = `${process.env.REACT_APP_API_URL}/issued-box/update-boxes-status`;

      this.props
        .updateIssuedBoxesStatusAction(url, [
          {
            issuedBoxID: this.issuedBoxID,
            statusID: statusID
          }
        ])
        .then(r => {
          resolve(r);
        });
    });
  }

  getBoxTimeLine() {
    return new Promise((resolve, reject) => {
      const url = `${process.env.REACT_APP_API_URL}issued-box/history/${this.issuedBoxID}`;
      this.props
        .getIssuedBoxTimeLineAction(url)
        .then(r => {
          resolve(r);
        })
        .catch(e => alert(e));
    });
  }

  handleAppendTag(tag) {
    const cloneState = { ...this.state };
    cloneState.scanTags.push({ ...tag });
    cloneState.issuedBoxClone.tags.push({ ...tag });
    cloneState.issuedBoxClone.quantity = cloneState.issuedBoxClone.tags.length;
    console.log(cloneState);
    this.setState(cloneState);
  }

  setNextStatus(statusID) {
    const newStatus = { name: "", id: -1 };
    switch (statusID) {
      case 1:
        newStatus["name"] = "Complete Box";
        newStatus["id"] = 2;
        break;

      case 2:
        newStatus["name"] = "Print Labels";
        newStatus["id"] = 3;
        break;

      case 3:
        newStatus["name"] = "Sent To Press";
        newStatus["id"] = 4;
        break;

      case 4:
        newStatus["name"] = "Verify Kitted Tags";
        newStatus["id"] = 5;
        break;

      case 5:
        newStatus["name"] = "Export Box";
        newStatus["id"] = 6;
        break;
      case 6:
      case 7:
        newStatus["name"] = "Issue Box";
        newStatus["id"] = 7;
        break;

      default:
        break;
    }

    return newStatus;
  }

  render() {
    return (
      <React.Fragment>
        <div className="actions">
          {this.state.activeForm === "list" ? (
            <IssuedBoxDetailActions
              isFormValid={this.state.isFormValid}
              onLinkClick={this.handleLinkClick.bind(this)}
              disableAssignTags={this.props.issuedBox.initialAssigned}
              nextStatus={this.state.nextStatus.name}
              disableSerialList={this.props.issuedBox.statusID === 1}
              disableCompleteBox={
                this.state.issuedBoxClone.statusID === 7 ||
                this.state.issuedBoxClone.quantity === 0
              }
            ></IssuedBoxDetailActions>
          ) : (
            <Actions
              isFormValid={this.state.isFormValid}
              onLinkClick={this.handleClick.bind(this)}
              disableSave={false}
            ></Actions>
          )}
        </div>

        {this.state.showAlert && this.state.showAlert()}

        {this.state.activeForm === "list" ? (
          <div className="title-summary-wrapper">
            <div className="content-title">
              <h3>{`BoxID ${this.issuedBoxID}`}</h3>
            </div>
            <div className="top-buffer">
              <TabContext value={this.state.selectedTab}>
                <Box
                  sx={{
                    borderBottom: 0,
                    borderColor: "divider",
                    width: "25%"
                  }}
                >
                  <Tabs
                    value={this.state.selectedTab}
                    variant="fullWidth"
                    indicatorColor="primary"
                    centered
                    onChange={(e, newValue) =>
                      this.handleChange.call(this, {
                        name: "tab",
                        value: newValue
                      })
                    }
                    aria-label="basic tabs example"
                  >
                    <Tab label="Summary" style={{ pading: 0, margin: 0 }} />
                    <Tab label="History" style={{ pading: 0, margin: 0 }} />
                    <Tab label="Tags" style={{ pading: 0, margin: 0 }} />
                  </Tabs>
                </Box>
                {this.state.selectedTab === 0 && (
                  <TabPanel
                    sx={{
                      width: "80%",
                      paddingLeft: 0,
                      paddingTop: 0
                    }}
                    value={this.state.selectedTab}
                    index={0}
                  >
                    <IssuedBoxItem
                      issuedBox={this.state.issuedBoxClone}
                      shipment={this.props.shipment}
                      statusID={this.state.statusID}
                      distributors={this.props.distributors}
                      distributorID={this.state.distributorID}
                      onChange={this.handleChange.bind(this)}
                    ></IssuedBoxItem>
                  </TabPanel>
                )}
                {this.state.selectedTab === 1 && (
                  <TabPanel
                    sx={{
                      width: "100%",
                      paddingLeft: 0,
                      paddingTop: 0
                    }}
                    value={this.state.selectedTab}
                    index={1}
                  >
                    <IssuedBoxTimeLine
                      issuedBoxHistory={this.props.issuedBoxHistory}
                    ></IssuedBoxTimeLine>
                  </TabPanel>
                )}
                {this.state.selectedTab === 2 && (
                  <TabPanel
                    sx={{
                      width: "100%",
                      paddingLeft: 0,
                      paddingTop: 0
                    }}
                    value={this.state.selectedTab}
                    index={2}
                  >
                    <IssuedBoxTags
                      issuedBox={this.state.issuedBoxClone}
                      columns={this.columns}
                      onLinkClick={this.handleClick.bind(this)}
                    ></IssuedBoxTags>
                  </TabPanel>
                )}
              </TabContext>
            </div>
          </div>
        ) : (
          <IssuedBoxScanTags
            columns={this.columns}
            appendTag={this.handleAppendTag.bind(this)}
            scanTags={this.state.scanTags}
            tags={this.state.issuedBoxClone.tags}
          ></IssuedBoxScanTags>
        )}
      </React.Fragment>
    );
  }
}

const mapStateToProps = state => {
  return {
    issuedBox: state.issuedBox.issuedBox,
    shipment: state.shipment.shipment,
    issuedBoxHistory: state.issuedBox.issuedBoxHistory,
    distributors: state.lookup.distributors
  };
};

const mapDispatchToProps = dispatch => ({
  getIssuedBoxAction: url => dispatch(getIssuedBox(url)),
  getShipmentAction: url => dispatch(getShipment(url)),
  updateIssuedBoxesStatusAction: (url, payload) =>
    dispatch(updateIssuedBoxesStatus(url, payload)),
  getIssuedBoxTimeLineAction: url => dispatch(getIssuedBoxTimeLine(url)),
  createUpdateIssueBoxAction: (url, payload) =>
    dispatch(createUpdateIssueBox(url, payload)),
  downloadSerialListAction: url => dispatch(downloadSerialList(url)),
  getDistributorsAction: url => dispatch(getDistributors(url)),
  updateIssuedBoxAction: (url, payload) =>
    dispatch(updateIssuedBox(url, payload))
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(IssuedBoxSummary);
