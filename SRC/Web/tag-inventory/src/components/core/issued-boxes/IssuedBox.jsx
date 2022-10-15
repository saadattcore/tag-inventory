import React, { Component } from "react";
import { connect } from "react-redux";
import { PropTypes } from "prop-types";
import {
  getReceivedBox,
  updateScanBoxTags
} from "../../../actions/receivedBoxActions";

import Actions from "../../common/reuseable/Actions";
import IssuedBoxDetail from "./IssuedBoxDetail";
import IssuedBoxScanTags from "./IssuedBoxScanTags";
import {
  createUpdateIssueBox,
  updateIssueBoxTags,
  getIssuedBox
} from "../../../actions/issuedBoxActions";
import { renderAlert } from "../../common/ui-controls/withAlert";
import config from "../../../config.json";

class IssuedBox extends Component {
  constructor(props) {
    super(props);

    this.columns = [
      {
        path: "tagID",
        title: "Tag ID",
        key: 1
      },
      { path: "tagNumber", title: "Tag Number", key: 2 },
      { path: "serialNumber", title: "Serial Number", key: 3 },
      { path: "status", title: "Tag Status", key: 4 },
      { path: "receivedBoxID", title: "Received Box ID", key: 5 },
      { path: "tagType", title: "Tag Type", key: 6 }
    ];

    this.state = {
      receivedBoxID: "",
      activateForm: "Detail",
      displayTable: false,
      alertOpen: false,
      dialogOpen: false,
      content: "",
      showScanTagActionBar: false,
      isFormValid: false,
      issuedBox: {},
      disableSave: false,
      disableSecondBtn: true,
      disableFirstBtn: false,
      disableActionBar: true,
      issuedBoxID: 0,
      scanTags: []
    };
  }

  componentDidMount() {
    const issuedBoxID = this.props.match.params.issuedBoxID;

    const url = `${process.env.REACT_APP_API_URL}issued-box/` + issuedBoxID;

    if (issuedBoxID > 0) {
      this.props.getIssuedBoxAction(url).then(issuedBox => {
        const cloneState = { ...this.state };

        cloneState.issuedBoxID = issuedBox.issuedBoxID;
        cloneState.receivedBoxID = issuedBox.receivedBoxID;
        cloneState.disableFirstBtn = true;
        cloneState.disableSecondBtn = false;
        cloneState.issuedBox = issuedBox;
        const url = `${process.env.REACT_APP_API_URL}received-box/${cloneState.receivedBoxID}`;
        this.props.getReceivedBoxAction(url).then(r => {
          this.setState(cloneState);
        });
      });
    }
  }

  handleClick({ target }) {
    const cloneState = { ...this.state };

    switch (target.name) {
      case "btnOne":
        this.validateReceivedBox()
          .then(r => {
            const url = `${process.env.REACT_APP_API_URL}issued-box`;

            this.props
              .createUpdateIssueBoxAction(url, {
                receivedBoxID: r.receivedBoxID,
                statusID: 1
              })
              .then(r => {
                cloneState.disableSecondBtn = false;
                cloneState.issuedBox = { ...this.props.issuedBox };
                //alert("Issued box created sucessfully");

                this.displayAlert(
                  "Issued box created sucessfully.",
                  "info",
                  "",
                  0
                ).then(r => {
                  this.setState(cloneState);
                });
              });
          })
          .catch(error => {
            console.log(error);
            if (error && error.status === 404) {
              this.displayAlert("Received box does not exist.", "error", "", 0);

              //alert("Received box does not exist.");
            }
          });

        break;

      case "btnTwo":
        cloneState.issuedBox = { ...this.props.issuedBox };
        cloneState.issuedBox.tags = [
          ...this.props.receivedBox.boxTags
            .filter(t => t.statusID === 1)
            .slice(0, config.boxCount)
        ];
        cloneState.issuedBox.quantity = cloneState.issuedBox.tags.length;
        cloneState.dialogOpen = true;
        this.setState(cloneState);
        break;
    }
  }

  handleChange({ target }) {
    const cloneState = { ...this.state };

    switch (target.name) {
      case "txtReceivedBox":
        cloneState.receivedBoxID = target.value;
        console.log(cloneState.receivedBoxID);
        break;
    }

    this.setState(cloneState);
  }

  validateReceivedBox() {
    return new Promise((resolve, reject) => {
      const cloneState = { ...this.state };
      const url = `${process.env.REACT_APP_API_URL}received-box/${cloneState.receivedBoxID}`;

      this.props
        .getReceivedBoxAction(url)
        .then(response => {
          console.log(response);

          if (response.issuedBoxCreated) {
            /*     cloneState.alertOpen = true;
            cloneState.content =
              "Issued Box already created using this box. Please select another received box to continue";

            this.setState(cloneState); */

            this.displayAlert(
              "Issued Box already created using this box. Please select another received box to continue.",
              "error",
              "",
              0
            );

            reject(false);
          }

          if (
            response.statusID === 0 ||
            response.boxTags.filter(t => t.statusID === 1).length === 0
          ) {
            this.displayAlert(
              "This received box not verified. Please scan all tags then continue.",
              "error",
              "",
              0
            );

            /*    cloneState.alertOpen = true;
            cloneState.content =
              "This received box not verified. Please scan all tags then continue";

            this.setState(cloneState); */
            reject(false);
          }
          resolve(response);
        })
        .catch(error => {
          reject(error);
        });
    });
  }

  handleDialogClose(arg, reason) {
    if (reason === "clickaway") {
      return;
    }
    const cloneState = { ...this.state };

    switch (arg.target.elementType) {
      case "alert":
        cloneState.alertOpen = false;
        this.setState(cloneState);
        break;
      case "dialog":
        if (arg.target.action === "continue") {
          cloneState.dialogOpen = false;
          cloneState.displayTable = true;
          cloneState.showScanTagActionBar = true;
          cloneState.isFormValid = true;
          this.setState(cloneState);
        } else {
          cloneState.dialogOpen = false;
          this.setState(cloneState);
        }
        break;
      case "contextmenu":
        break;
    }
  }

  displayAlert(content, type) {
    return new Promise((resolve, reject) => {
      const stateClone = { ...this.state };
      stateClone.showAlert = renderAlert.bind(null, type, content, true, e => {
        stateClone.showAlert = null;
        this.setState(stateClone);
        resolve("fulfilled");
      });

      this.setState(stateClone);
    });
  }

  saveTags(issuedBoxStatusID, op) {
    return new Promise((resolve, reject) => {
      switch (op) {
        case "add":
          this.props
            .createUpdateIssueBoxAction(
              `${process.env.REACT_APP_API_URL}issued-box`,
              { ...this.state.issuedBox }
            )
            .then(r => {
              resolve(true);
            });
          break;
        default:
          alert("Unknown action");
          break;
      }
    });
  }

  handleAppendTag(tag) {
    const cloneState = { ...this.state };
    cloneState.issuedBox.tags.push({ ...tag });
    cloneState.issuedBox.quantity = cloneState.issuedBox.tags.length;
    cloneState.scanTags.push({ ...tag });
    console.log(cloneState);
    this.setState(cloneState);
  }

  renderDialog() {
    const cloneState = { ...this.state };

    return (
      <div className="modelPopup" style={{ paddingLeft: 30 }}>
        <div>
          <p style={{ fontSize: 16, fontWeight: 400 }}>
            Received box includes {this.props.receivedBox.boxTags.length} tags
          </p>
        </div>
        <ul style={{ marginLeft: 40 }}>
          <li>
            <p style={{ fontSize: 16, fontWeight: 400 }}>
              {
                this.props.receivedBox.boxTags.filter(t => t.statusID === 1)
                  .length
              }{" "}
              {"Tags have been Verified and Scanned"}
            </p>
          </li>
          <li>
            <p style={{ fontSize: 16, fontWeight: 400 }}>
              {
                this.props.receivedBox.boxTags.filter(t => t.statusID === 4)
                  .length
              }
              {" tags are missing"}
            </p>
          </li>
        </ul>
        <div>
          <p style={{ fontSize: 16, fontWeight: 400 }}>
            {this.props.receivedBox.boxTags.length > config.boxCount
              ? `The capacity of issued box is ${config.boxCount}. Only first ${config.boxCount} 
              verified and scanned tags will be assigned to new Box`
              : `Only ${
                  this.props.receivedBox.boxTags.filter(t => t.statusID === 1)
                    .length
                } verified and scanned tags will be assigned to new Box`}
          </p>
        </div>
      </div>
    );
  }

  handleLinkClick(arg) {
    console.log(arg);
    const cloneState = { ...this.state };

    switch (arg.target.action) {
      case "scan":
        cloneState.activateForm = "ScanVarifiedTags";
        this.setState(cloneState);
        break;
      case "back":
        this.props.history.push("/issued-box");
        break;
      case "saveclose":
      case "save":
        if (this.state.activateForm === "ScanVarifiedTags") {
          cloneState.activateForm = "Detail";
          this.setState(cloneState);
        } else {
          this.saveTags(1, "add").then(r => {
            if (arg.target.action === "saveclose") {
              this.props.history.push("/issued-box");
            } else {
              // alert("Sucessfully created issued box with tags");
              this.displayAlert(
                "Please enter received box id to fetch relevant tags.",
                "info",
                "",
                0
              );
              cloneState.isFormValid = false;
            }
            this.setState(cloneState);
          });
        }

        break;

      case "assign":
        if (this.props.newScanTags.length === 0) {
          // alert("No new tag being scanned");
          this.displayAlert(
            "Please enter received box id to fetch relevant tags.",
            "error",
            "",
            0
          );
          return;
        }

        cloneState.issuedBox.boxTags.append([...this.props.newScanTags]);
        cloneState.issuedBox.quantity = cloneState.issuedBox.boxTags.length;
        let issuedBoxStatus = 1;
        if (cloneState.issuedBox.boxTags === config.boxCount) {
          issuedBoxStatus = 2;
        }
        this.saveTags(issuedBoxStatus, "add").then(r => {
          this.setState(cloneState);
        });
        break;
    }
  }

  displayAlert(content, type) {
    return new Promise((resolve, reject) => {
      const stateClone = { ...this.state };
      stateClone.showAlert = renderAlert.bind(null, type, content, true, e => {
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
          <Actions
            isFormValid={this.state.isFormValid}
            onLinkClick={this.handleLinkClick.bind(this)}
            disableSave={this.state.disableSave}
          ></Actions>
        </div>
        {this.state.showAlert && this.state.showAlert()}
        {this.state.activateForm === "Detail" ? (
          <IssuedBoxDetail
            issuedBox={this.state.issuedBox}
            columns={this.columns}
            onButtonClick={this.handleClick.bind(this)}
            onTextChange={this.handleChange.bind(this)}
            onHandleDialogClose={this.handleDialogClose.bind(this)}
            onDisplayAlert={this.displayAlert.bind(this)}
            onRenderDialog={this.renderDialog.bind(this)}
            displayTable={this.state.displayTable}
            alertOpen={this.state.alertOpen}
            dialogOpen={this.state.dialogOpen}
            content={this.state.content}
            showScanTagActionBar={this.state.showScanTagActionBar}
            onLinkClick={this.handleLinkClick.bind(this)}
            disableSave={this.state.disableSave}
            disableSecondBtn={this.state.disableSecondBtn}
            disableFirstBtn={this.state.disableFirstBtn}
            receivedBoxValue={this.state.receivedBoxID}
            title={
              this.props.issuedBox &&
              Object.keys(this.props.issuedBox).length > 0
                ? this.props.issuedBox.issuedBoxID
                : "New Box"
            }
          ></IssuedBoxDetail>
        ) : (
          <IssuedBoxScanTags
            columns={this.columns}
            appendTag={this.handleAppendTag.bind(this)}
            scanTags={this.state.scanTags}
            tags={this.state.issuedBox.tags}
          ></IssuedBoxScanTags>
        )}
      </React.Fragment>
    );
  }
}

IssuedBox.propTypes = {
  receivedBox: PropTypes.object.isRequired,
  searchCount: PropTypes.number.isRequired,
  totalCount: PropTypes.number.isRequired
};

const mapStateToProps = state => {
  return {
    receivedBox: state.receivedBox.receivedBox,
    issuedBox: state.issuedBox.issuedBox,
    newScanTags: state.issuedBox.scanTags
  };
};

const mapDispatchToProps = dispatch => ({
  getReceivedBoxAction: url => dispatch(getReceivedBox(url)),
  updateScanBoxTagsAction: (url, payload) =>
    dispatch(updateScanBoxTags(url, payload)),
  createUpdateIssueBoxAction: (url, payload) =>
    dispatch(createUpdateIssueBox(url, payload)),
  updateIssueBoxTagsAction: (url, payload) =>
    dispatch(updateIssueBoxTags(url, payload)),
  getIssuedBoxAction: url => dispatch(getIssuedBox(url))
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(IssuedBox);
