import React, { Component } from "react";
import { connect } from "react-redux";
import { PropTypes } from "prop-types";
import {
  getIssuedBox,
  updateIssueBoxTags
} from "../../../actions/issuedBoxActions";
import BarcodeReader from "react-barcode-reader";
import DialogComponent from "../../common/ui-controls/DialogComponent";
import Actions from "../../common/reuseable/Actions";
import Table from "../../common/ui-controls/Table";
import FieldSet from "../../common/reuseable/FieldSet";
import Snackbar from "@material-ui/core/Snackbar";
import Alert from "@material-ui/core/Alert";
import AlertTitle from "@material-ui/core/AlertTitle";
import match from "../tag/audio/match.mp3";
import mismatch from "../tag/audio/mismatch.mp3";
import config from "../../../config.json";

class VerifyKittedTags extends Component {
  constructor(props) {
    super(props);
    this.state = {
      issuedBoxID: "",
      issuedBox: { tags: [] },
      scannedTags: [],
      dialogOpen: false,
      notFoundTag: -1,
      otherBoxTag: {},
      alertOpen: false,
      alertType: "info",
      alertTitle: "Info",
      alertDuration: 2000,
      content: ""
    };

    this.columns = [
      {
        path: "tagID",
        title: "Tag ID",
        key: 1
      },
      { path: "tagNumber", title: "Tag Number", key: 2 },
      { path: "serialNumber", title: "Serial Number", key: 3 },
      { path: "status", title: "Tag Status", key: 4 },

      {
        path: "kitVisualCheckStatusID",
        title: "Visual Check",
        key: 5,
        content: (arg, tag) => {
          return (
            <div
              className={
                tag.kitVisualCheckStatusID !== 1 ? "visual-check-container" : ""
              }
            >
              <div>
                <input
                  type="checkbox"
                  name="checkBoxVisualCheck"
                  id="checkBoxVisualCheck"
                  disabled={tag.kitVisualCheckStatusID === 1}
                  checked={tag.kitVisualCheckStatusID === 1}
                  onChange={e =>
                    this.handleChange.call(this, {
                      target: {
                        name: "checkBoxVisualCheck",
                        tagNumber: tag.tagNumber
                      }
                    })
                  }
                />
              </div>
              <div>
                {tag.kitVisualCheckStatusID !== 1 && (
                  <select
                    name="ddlVisualCheck"
                    id="ddlVisualCheck"
                    className="scan-tag-visualcheck-select"
                    value={tag.kitVisualCheckStatusID}
                    onChange={e =>
                      this.handleChange.call(this, {
                        target: {
                          name: "ddlVisualCheck",
                          value: e.target.value,
                          tagNumber: tag.tagNumber
                        }
                      })
                    }
                  >
                    <option value={0}>Not Verified</option>
                    <option value={2}>Bar Code Missing</option>
                  </select>
                )}
              </div>
            </div>
          );
        }
      },

      {
        path: "kitRFIDCheckStatusID",
        title: "RFID Check",
        key: 6,
        content: (arg, tag) => {
          return (
            <div
              className={
                tag.kitRFIDCheckStatusID !== 1 ? "visual-check-container" : ""
              }
            >
              <div>
                <input
                  type="checkbox"
                  name="checkBoxRFIDCheck"
                  id="checkBoxRFIDCheck"
                  disabled={tag.kitRFIDCheckStatusID === 1}
                  checked={tag.kitRFIDCheckStatusID === 1}
                  onChange={e =>
                    this.handleChange.call(this, {
                      target: {
                        name: "checkBoxRFIDCheck",
                        tagNumber: tag.tagNumber
                      }
                    })
                  }
                />
              </div>
              <div>
                {tag.kitRFIDCheckStatusID !== 1 && (
                  <select
                    name="ddlRFIDCheck"
                    id="ddlRFIDCheck"
                    className="scan-tag-visualcheck-select"
                    value={tag.kitRFIDCheckStatusID}
                    onChange={e =>
                      this.handleChange.call(this, {
                        target: {
                          name: "ddlRFIDCheck",
                          value: e.target.value,
                          tagNumber: tag.tagNumber
                        }
                      })
                    }
                  >
                    <option value={0}>Not Scanned</option>
                    <option value={2}>TagNumber Missing</option>
                  </select>
                )}
              </div>
            </div>
          );
        }
      },

      { path: "issuedBoxID", title: "Issued Box ID", key: 7 }
    ];
  }

  componentDidMount() {
    //this.displayDialog(this.renderDialog.bind(this));

    let evtSource = new EventSource(
      process.env.REACT_APP_API_URL + "TagReader"
    );

    console.log(evtSource.readyState);

    evtSource.onopen = event => {
      console.log(event);
    };

    evtSource.onerror = event => {
      console.log(event);
    };

    evtSource.onmessage = event => {
      //alert("received tag scan from reader");
      const readerScan = this.readerScan.bind(this);
      console.log(event.data);
      readerScan(JSON.parse(event.data));
      //this.updateReadTagScanner(event.data);
    };
  }

  displayAlert(content, type, title, duration) {
    const cloneState = { ...this.state };
    cloneState.alertOpen = true;
    cloneState.content = content;
    cloneState.alertType = type;
    cloneState.alertTitle = title;
    cloneState.alertDuration = duration;
    this.setState(cloneState);
  }

  handleChange({ target }) {
    console.log(target);
    const cloneState = { ...this.state };
    let scanTag;

    if (cloneState.scannedTags.length > 0) {
      scanTag = cloneState.scannedTags.find(
        tag => tag.tagNumber === target.tagNumber
      );
    }

    switch (target.name) {
      case "ddlVisualCheck":
        scanTag.kitVisualCheckStatusID = scanTag.kitVisualCheckStatusID = parseInt(
          target.value
        );
        break;
      case "checkBoxVisualCheck":
        scanTag.kitVisualCheckStatusID =
          scanTag.kitVisualCheckStatusID === 1 ? 0 : 1;
        break;
      case "ddlRFIDCheck":
        scanTag.kitRFIDCheckStatusID = scanTag.kitRFIDCheckStatusID = parseInt(
          target.value
        );
        break;
      case "checkBoxRFIDCheck":
        scanTag.kitRFIDCheckStatusID =
          scanTag.kitRFIDCheckStatusID === 1 ? 0 : 1;
        break;

      default:
        const regex = RegExp("^[0-9]+$");

        let txtBoxValue = target.value;
        txtBoxValue = txtBoxValue.toString();

        if (regex.test(txtBoxValue)) {
          cloneState.issuedBoxID = target.value;
        } else {
          cloneState.issuedBoxID = "";
        }
        break;
    }
    this.setState(cloneState);
  }

  handleClick(arg) {
    const cloneState = { ...this.state };
    const url = `${process.env.REACT_APP_API_URL}issued-box/${cloneState.issuedBoxID}`;

    this.props
      .getIssuedBoxAction(url)
      .then(response => {
        if (response.statusID !== 4) {
          alert(
            "This issued box was not send to printing press. All issued box must be labeled and send to printing press."
          );
          return;
        }

        if (response.statusID === 5) {
          alert("This issued box already received.");
          return;
        }

        cloneState.issuedBox = { ...response };
        cloneState.content = `Issued box contains ${response.tags.length} kitted tags`;
        cloneState.alertOpen = true;
        this.setState(cloneState);
      })
      .catch(err => {
        alert(`Issued box with id=${cloneState.issuedBoxID} does not exist`);
      });
  }

  readerScan({ message, serialNumber, messageType }) {
    this.displayAlert(`tag number = ${message}`, "info", "", 1000);

    const stateClone = { ...this.state };
    console.log(message);

    if (stateClone.issuedBox.tags.find(t => t.tagNumber === message)) {
      if (!stateClone.scannedTags.find(t => t.tagNumber === message)) {
        const tag = {
          ...stateClone.issuedBox.tags.find(t => t.tagNumber === message)
        };

        tag.kitRFIDCheckStatusID = 1;
        stateClone.scannedTags.splice(0, 0, { ...tag });
        this.setState(stateClone);
      } else {
        const tag = stateClone.scannedTags.find(t => t.tagNumber === message);
        console.log(tag);
        tag.kitRFIDCheckStatusID = 1;
        if (!tag.isBeepedAlready && tag.kitVisualCheckStatusID === 1) {
          this.beep("match");
          tag.isBeepedAlready = true;
        }
      }
    } else {
      if (!stateClone.scannedTags.find(t => t.tagNumber === message)) {
        const url = `${
          process.env.REACT_APP_API_URL
        }tag/list?pageSize=${1}&pageNumber=${1}&tagNumber=${message}`;

        this.props
          .getIssuedBoxAction(url)
          .then(response => {
            const tag = response.data[0];
            tag.kitRFIDCheckStatusID = 1;
            stateClone.otherBoxTag = { ...tag };
            stateClone.dialogOpen = true;
            //stateClone.notFoundTag = message;
            this.setState(stateClone);
          })
          .catch(error => {
            console.log(error.response.status);
            alert("This tag does not exist in the system.");
          });
      } else {
        const tag = stateClone.scannedTags.find(t => t.tagNumber === message);
        tag.kitRFIDCheckStatusID = 1;
        if (!tag.isBeepedAlready && tag.kitVisualCheckStatusID === 1) {
          this.beep("match");
          tag.isBeepedAlready = true;
        }
      }
    }

    this.setState(stateClone);
  }

  handleScan(message) {
    const stateClone = { ...this.state };
    console.log(message);

    if (stateClone.issuedBox.tags.find(t => t.tagNumber === message)) {
      if (!stateClone.scannedTags.find(t => t.tagNumber === message)) {
        const tag = {
          ...stateClone.issuedBox.tags.find(t => t.tagNumber === message)
        };

        tag.kitVisualCheckStatusID = 1;
        stateClone.scannedTags.splice(0, 0, { ...tag });

        this.setState(stateClone);
      } else {
        const tag = stateClone.scannedTags.find(t => t.tagNumber === message);
        console.log(tag);
        tag.kitVisualCheckStatusID = 1;
        if (!tag.isBeepedAlready && tag.kitRFIDCheckStatusID === 1) {
          this.beep("match");
          tag.isBeepedAlready = true;
        }
      }
    } else {
      if (!stateClone.scannedTags.find(t => t.tagNumber === message)) {
        const url = `${
          process.env.REACT_APP_API_URL
        }tag/list?pageSize=${1}&pageNumber=${1}&tagNumber=${message}`;

        this.props
          .getIssuedBoxAction(url)
          .then(response => {
            const tag = response.data[0];
            tag.kitVisualCheckStatusID = 1;
            stateClone.otherBoxTag = { ...tag };
            stateClone.dialogOpen = true;
            //stateClone.notFoundTag = message;
            this.setState(stateClone);
          })
          .catch(error => {
            console.log(error.response.status);
            alert("This tag does not exist in the system.");
          });
      } else {
        const tag = stateClone.scannedTags.find(t => t.tagNumber === message);
        tag.kitVisualCheckStatusID = 1;

        if (!tag.isBeepedAlready && tag.kitRFIDCheckStatusID === 1) {
          this.beep("match");
          tag.isBeepedAlready = true;
        }
      }
    }

    this.setState(stateClone);
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
          cloneState.otherBoxTag.issuedBoxID = cloneState.issuedBox.issuedBoxID;
          //cloneState.otherBoxTag.kitVisualCheckStatusID = 1;
          cloneState.scannedTags.push({ ...cloneState.otherBoxTag });
          cloneState.otherBoxTag = {};
          cloneState.dialogOpen = false;
        } else {
          cloneState.dialogOpen = false;
        }
        this.setState(cloneState);
        break;
      case "contextmenu":
        break;
    }
  }

  renderDialog() {
    const cloneState = { ...this.state };

    return (
      <div className="modelPopup" style={{ paddingLeft: 30 }}>
        <div>
          <p style={{ fontSize: 16, fontWeight: 400 }}>
            The tag does not belongs to this box.
          </p>
        </div>
        <div>
          <p style={{ fontSize: 16, fontWeight: 400 }}>
            {`This tag belongs to ${cloneState.otherBoxTag.issuedBoxID}. Press Keep to keep the kit in the scan list or click Move to exclude  `}
          </p>
        </div>
      </div>
    );

    // end assign tags to issued box
  }

  handleLinkClick(arg) {
    console.log(arg);
    const cloneState = { ...this.state };
    switch (arg.target.action) {
      case "scan":
        cloneState.activateForm = "ScanVarifiedTags";
        break;
      case "saveclose":
        const scannedTags = cloneState.scannedTags;
        const actualBoxTags = cloneState.issuedBox.tags;

        scannedTags.forEach(t => {
          if (t.kitVisualCheckStatusID !== 1 || t.kitRFIDCheckStatusID !== 1) {
            t.statusID = 2;
            t.issuedBoxID = null;
          }
        });

        let boxToUpdate = { ...cloneState.issuedBox };
        boxToUpdate = { ...boxToUpdate, tags: [...cloneState.scannedTags] };

        const missingTags = actualBoxTags.filter(
          t => !scannedTags.find(st => st.tagNumber === t.tagNumber)
        );

        let quantity = boxToUpdate.tags.filter(t => t.statusID === 1).length;

        boxToUpdate.quantity = quantity;

        console.log(boxToUpdate);

        if (missingTags.length === 0) {
          if (boxToUpdate.quantity === config.boxCount) {
            boxToUpdate.statusID = 5;
          }
          boxToUpdate.tags.forEach(t => {
            if (t.statusID === 1) {
              t.statusID = 6;
            }
          });
        } else {
          boxToUpdate.statusID = 4;

          boxToUpdate.tags.forEach(t => {
            if (t.statusID === 1) {
              t.statusID = 6;
            }
          });

          missingTags.forEach(t => {
            t.statusID = 5;
            t.issuedBoxID = null;
            boxToUpdate.tags.push({ ...t });
          });

          console.log(missingTags);

          let ms = `Issued box includes ${actualBoxTags.length}\n 
          Only ${scannedTags.length} have been verified\n 
          The box quantity will be updated to ${scannedTags.length} 
          and status will not be updated to received.`;

          alert(ms);
        }

        const url = `${process.env.REACT_APP_API_URL}issued-box?updateIssuedBoxKits=true`;

        console.log(boxToUpdate);

        this.props
          .updateIssueBoxTagsAction(url, boxToUpdate)
          .then(response => {
            alert("Sucessfully updated issued box and associated tags status");
          })
          .catch(err => {});

        break;

      case "assign":
        this.saveTags(2, "update");
        break;
    }
    this.setState(cloneState);
  }

  beep(type) {
    let beep;
    switch (type) {
      case "match":
        beep = new Audio(match);
        break;
      case "mismatch":
        beep = new Audio(mismatch);
        break;
    }

    beep.play();
  }

  render() {
    return (
      <React.Fragment>
        {this.state.dialogOpen && (
          <DialogComponent
            title="SUMMARY"
            open={this.state.dialogOpen}
            content={this.renderDialog()}
            actions={[
              {
                action: "continue",
                elementType: "dialog",
                text: "Keep"
              },
              {
                action: "cancel",
                elementType: "dialog",
                text: "Move"
              }
            ]}
            onDialogClose={this.handleDialogClose.bind(this)}
          ></DialogComponent>
        )}

        <Snackbar
          open={this.state.alertOpen}
          onClose={e =>
            this.handleDialogClose.call(this, {
              target: { action: "close", elementType: "alert" }
            })
          }
          anchorOrigin={{ vertical: "bottom", horizontal: "left" }}
          autoHideDuration={this.state.alertDuration}
        >
          <Alert
            severity={this.state.alertType}
            onClose={e =>
              this.handleDialogClose.call(this, {
                target: { action: "close", elementType: "alert" }
              })
            }
          >
            <AlertTitle>{this.state.alertTitle}</AlertTitle>
            <strong>{this.state.content}</strong>
          </Alert>
        </Snackbar>

        <BarcodeReader onError={null} onScan={this.handleScan.bind(this)} />
        <div className="actions">
          <Actions
            isFormValid={true}
            onLinkClick={this.handleLinkClick.bind(this)}
          ></Actions>
        </div>
        <div className="content-area-main">
          <div className="content-title">
            <h4>New Box</h4>
          </div>
          <div className="row top-buffer" style={{ marginLeft: 3 }}></div>

          <div className="row">
            <div className="col-md-4">
              <FieldSet
                inputValue={this.state.issuedBoxID}
                onTextChange={this.handleChange.bind(this)}
                onButtonClick={this.handleClick.bind(this)}
                form="issuedBox"
                buttonCaption="Start Verification"
                labelCaption="Issued Box"
              ></FieldSet>
            </div>
          </div>
          <div className="row top-buffer">
            <div className="list-container">
              <Table
                columns={this.columns}
                data={this.state.scannedTags}
                searchVisible={false}
              ></Table>
            </div>
          </div>
        </div>
      </React.Fragment>
    );
  }
}

VerifyKittedTags.propTypes = {
  issuedBox: PropTypes.object.isRequired
};

const mapStateToProps = state => {
  return {
    issuedBox: state.issuedBox.issuedBox
  };
};

const mapDispatchToProps = dispatch => ({
  getIssuedBoxAction: url => dispatch(getIssuedBox(url)),
  updateIssueBoxTagsAction: (url, payload) =>
    dispatch(updateIssueBoxTags(url, payload))
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(VerifyKittedTags);
