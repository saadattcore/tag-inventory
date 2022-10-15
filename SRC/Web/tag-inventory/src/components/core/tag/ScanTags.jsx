import React, { Component } from "react";
import BarcodeReader from "react-barcode-reader";
import { connect } from "react-redux";
import { PropTypes } from "prop-types";
import {
  getReceivedBox,
  updateScanBoxTags
} from "../../../actions/receivedBoxActions";
import config from "../../../config.json";
import Actions from "../../common/reuseable/Actions";
import Table from "../../common/ui-controls/Table";
import Alert from "@material-ui/core/Alert";
import AlertTitle from "@material-ui/core/AlertTitle";
import Stack from "@material-ui/core/Stack";
import Button from "@material-ui/core/Button";
import Snackbar from "@material-ui/core/Snackbar";
import FieldSet from "../../common/reuseable/FieldSet";
import DialogComponent from "../../common/ui-controls/DialogComponent";
import Menu from "@material-ui/core/Menu";
import MenuItem from "@material-ui/core/MenuItem";
import _, { clone, indexOf } from "lodash";
import { ContactPageOutlined } from "@material-ui/icons";
import { Link } from "@material-ui/core";
import match from "./audio/match.mp3";
import mismatch from "./audio/mismatch.mp3";
import { getTags } from "../../../actions/tagActions";
import { renderAlert } from "../../common/ui-controls/withAlert";

class ScanTags extends Component {
  constructor(props) {
    super(props);
    this.state = {
      scanTags: [],
      receivedBox: { receivedBoxID: -1, receivedBoxStatus: -1, tags: [] },
      currentTag: null,
      rfidReadDone: false,
      barCodeReadDone: false,
      receivedBoxID: "",
      receivedBoxStatus: 1,
      displayTable: false,
      visualCheckSwitch: true,
      alertOpen: false,
      dialogOpen: false,
      contextMenuOpen: false,
      content: "",
      saveAction: "",
      wrongTagsLookup: [{ wrongTag: "", correctTag: "" }],
      alertType: "info",
      alertTitle: "Info",
      alertDuration: 2000,
      additionalTags: [],
      showAlert: null
    };

    this.columns = [
      {
        path: "tagID",
        title: "Tag ID",
        key: 1
      },
      { path: "tagNumber", title: "Tag Number", key: 2 },
      { path: "serialNumber", title: "Serial Number", key: 3 },
      {
        path: "isImported",
        title: "Is Imported",
        key: 4,
        content: arg => {
          return <input type="checkbox" disabled checked={arg} />;
        }
      },
      {
        path: "visualCheckStatusID",
        title: "Visual Check",
        key: 5,
        className: this.getCssClassName.bind(this),
        content: (arg, tag) => {
          //if (tag.visualCheckStatusID === 0) tag.visualCheckStatusID = 1;
          return (
            <div
              className={
                tag.visualCheckStatusID !== 1 ? "visual-check-container" : ""
              }
            >
              <div>
                <input
                  type="checkbox"
                  name="checkBoxVisualCheck"
                  id="checkBoxVisualCheck"
                  checked={tag.visualCheckStatusID === 1}
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
                {tag.visualCheckStatusID !== 1 && (
                  <select
                    name="ddlVisualCheck"
                    id="ddlVisualCheck"
                    className="scan-tag-visualcheck-select"
                    value={tag.visualCheckStatusID}
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
                    <option value={3}>Multiple Barcodes</option>
                    <option value={4}>Cut From Right</option>
                    <option value={5}>Cut From Top</option>
                    <option value={6}>Mismatch</option>
                  </select>
                )}
              </div>
            </div>
          );
        }
      },
      {
        path: "rfidCheck",
        title: "RFID Check",
        key: 6,
        content: (arg, tag) => {
          return (
            <div>
              <div>
                <input
                  type="checkbox"
                  name="checkBoxRFID"
                  id="checkBoxRFID"
                  onChange={e =>
                    this.handleChange.call(this, {
                      target: {
                        name: "checkBoxRFID",
                        tagNumber: tag.tagNumber
                      }
                    })
                  }
                  disabled
                  checked={tag.rfidCheckStatusID === 1}
                ></input>
              </div>
              <div>
                {!tag.inEditMode && tag.rfidCheckStatusID !== 1 && (
                  <a
                    href="#"
                    title="edit"
                    onClick={e =>
                      this.handleChange.call(this, {
                        target: { name: "btnEdit", tagNumber: tag.tagNumber }
                      })
                    }
                  >
                    <i className="ms-Icon ms-font-md ms-Icon--FullWidthEdit"></i>
                  </a>
                  /*  <button
                    className="btn btn-primary"
                    name="btnEdit"
                    className="btn btn-primary"
                    id="btnEdit"
                    style={{ width: 70 }}
                    onClick={e =>
                      this.handleChange.call(this, {
                        target: { name: "btnEdit", tagNumber: tag.tagNumber }
                      })
                    }
                  >
                    Edit
                  </button> */
                )}

                {tag.inEditMode && tag.rfidCheckStatusID !== 1 && (
                  <select
                    name="ddlRFIDReasons"
                    id="ddlRFIDReasons"
                    className="scan-tag-visualcheck-select"
                    value={tag.rfidCheckStatusID}
                    disabled={
                      tag.rfidCheckStatusID === 3 && tag.appMarkMismatch
                    }
                    onChange={e =>
                      this.handleChange.call(this, {
                        target: {
                          name: "ddlRFIDReasons",
                          value: e.target.value,
                          tagNumber: tag.tagNumber
                        }
                      })
                    }
                  >
                    <option value={2}>Tag Number Missing</option>
                    <option value={3}>Mismatch</option>
                  </select>
                )}
              </div>
            </div>
          );
        }
      },
      {
        path: "",
        title: "Action",
        key: 8,
        content: (arg, tag) => {
          return (
            <React.Fragment>
              <a
                href="#"
                onClick={e =>
                  this.handleClick.call(this, {
                    target: {
                      action: "toggleContextMenu",
                      name: "btnContextMenu",
                      tagNumber: tag.tagNumber,
                      element: e.target
                    }
                  })
                }
              >
                <i className="ms-Icon ms-font-xl ms-Icon--MoreVertical"></i>
              </a>
              {tag.openContextMenu && (
                <div>
                  <Menu
                    id="long-menu"
                    MenuListProps={{
                      "aria-labelledby": "long-button"
                    }}
                    anchorEl={tag.anchorEl}
                    open={tag.openContextMenu}
                    onClose={e =>
                      this.handleMenuClose.call(this, {
                        target: {
                          element: e,
                          action: null,
                          tagNumber: tag.tagNumber
                        }
                      })
                    }
                    PaperProps={{
                      style: {
                        maxHeight: 48 * 4.5,
                        width: "20ch"
                      }
                    }}
                  >
                    {this.contextMenuOptions.map(option => (
                      <MenuItem
                        key={option}
                        //selected={option === "Pyxis"}
                        onClick={e =>
                          this.handleMenuClose.call(this, {
                            target: {
                              element: e,
                              action: option,
                              tagNumber: tag.tagNumber
                            }
                          })
                        }
                      >
                        {option}
                      </MenuItem>
                    ))}
                  </Menu>
                </div>
              )}
              {tag.openDialog && (
                <DialogComponent
                  title={tag.dialogTitle}
                  open={tag.openDialog}
                  content={
                    <input
                      type="text"
                      className="form-control"
                      value={
                        tag.contextMenuSelectedOption === "Serial"
                          ? tag.serialNumber
                          : tag.pIN
                      }
                      onChange={e => {
                        const stateClone = { ...this.state };
                        const t = stateClone.scanTags.find(
                          t => t.tagNumber === tag.tagNumber
                        );
                        t.contextMenuSelectedOption === "Serial"
                          ? (t.serialNumber = e.target.value)
                          : (t.pIN = e.target.value);
                        this.setState(stateClone);
                      }}
                    ></input>
                  }
                  actions={[
                    {
                      action: "contextmenu",
                      type: "contextmenu",
                      text: "Save"
                    }
                  ]}
                  onDialogClose={e => {
                    const stateClone = { ...this.state };
                    const t = stateClone.scanTags.find(
                      t => t.tagNumber === tag.tagNumber
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

    this.contextMenuOptions = ["Delete", "Edit Serial Number", "Edit PIN"];
  }

  componentDidMount() {
    //this.displayDialog(this.renderDialog.bind(this));

    let evtSource = new EventSource(
      process.env.REACT_APP_API_URL + "tagreader"
    );

    console.log(evtSource.readyState);

    evtSource.onopen = event => {
      console.log(event);
    };

    evtSource.onerror = event => {
      console.log(event);
    };

    evtSource.onmessage = event => {
      const readerScan = this.readerScan.bind(this);

      console.log(readerScan);
      console.log(event.data);

      readerScan(JSON.parse(event.data));
      //this.updateReadTagScanner(event.data);
    };

    if (this.props.match.params.receivedBoxID) {
      const cloneState = { ...this.state };
      cloneState.receivedBoxID = this.props.match.params.receivedBoxID;
      cloneState.displayTable = true;

      const url = `${
        process.env.REACT_APP_API_URL
      }received-box/list?pageSize=${250}&pageNumber=${1}&receivedBoxID=${
        cloneState.receivedBoxID
      }`;

      this.props
        .getReceivedBoxAction(url)
        .then(response => {
          console.log(response);
          cloneState.receivedBox = response.data[0];
          cloneState.displayTable = true;
          this.setState(cloneState);
        })
        .catch(error => {
          alert(error);
        });
    }
  }

  getCssClassName(colName, colvalue) {
    let retClassName = "";
    if (colName === "visualCheckStatusID" && colvalue !== 1) {
      retClassName = "scan-tag-table-visualcheck-td";
    }
    return retClassName;
  }

  async readerScan({ message, serialNumber, messageType }) {
    await this.displayAlert(`tag number = ${message}`, "info", "", 0);

    if (messageType !== "read") {
      this.displayAlert(message, "error", "", 0);
      //alert(message);
      return;
    }

    if (!this.state.receivedBoxID) {
      this.displayAlert("Please set received box id first", "error", "", 0);
      //alert("Please set received box id first");
      return;
    }

    if (this.state.receivedBox.boxTags.length === 0) {
      this.displayAlert(
        "Please set received box id and click the scan button to load tags for box",
        "error",
        "",
        0
      );
      /*   alert(
        "Please set received box id and click the scan button to load tags for box"
      ); */
      return;
    }

    const stateClone = { ...this.state };

    if (stateClone.receivedBox.boxTags.find(t => t.tagNumber === message)) {
      if (!stateClone.scanTags.find(t => t.tagNumber === message)) {
        const tag = {
          ...stateClone.receivedBox.boxTags.find(t => t.tagNumber === message)
        };
        tag.rfidCheckStatusID = 1;
        //stateClone.scanTags.push({ ...tag });
        stateClone.scanTags.splice(0, 0, { ...tag });
      } else {
        // alert("else");
        const tag = stateClone.scanTags.find(t => t.tagNumber === message);
        console.log(tag);
        tag.rfidCheckStatusID = 1;

        /*   const listAfterTagDel = stateClone.scanTags.filter(
          (t) => t.tagNumber !== tag.tagNumber
        ); */

        if (!tag.isBeepedAlready && tag.visualCheckStatusID === 1) {
          this.beep("match");
          tag.isBeepedAlready = true;
        }

        /*      listAfterTagDel.splice(0, 0, { ...tag });
        stateClone.scanTags = [...listAfterTagDel]; */
      }
    } else {
      const url = `${
        process.env.REACT_APP_API_URL
      }tag/list?pageSize=${1}&pageNumber=${1}&tagNumber=${message}`;

      this.props
        .getTagsAction(url)
        .then(response => {
          console.log(response);
          const tag = response[0];
          if (tag) {
            this.displayAlert(
              `This tag belongs to ${tag.receivedBoxID} box. Please remove this tag from ${this.state.receivedBoxID} box and move it to box ${tag.receivedBoxID}`,
              "error",
              "",
              0
            );

            /*    alert(
              `This tag belongs to ${tag.receivedBoxID} box. Please remove this tag from ${this.state.receivedBoxID} box and move it to box ${tag.receivedBoxID}`
            ); */
          }
        })
        .catch(error => {
          console.log("Promise emits error");
          let additionalTag = stateClone.additionalTags.find(
            t => t.tagNumber === message
          );

          if (!additionalTag) {
            additionalTag = this.createTag(message);
            additionalTag.rfidCheckStatusID = 1;
            stateClone.additionalTags.push({ ...additionalTag });
          } else {
            if (
              !stateClone.scanTags.find(t => t.tagNumber === message) &&
              additionalTag.rfidCheckStatusID === 0
            ) {
              additionalTag.rfidCheckStatusID = 1;

              if (
                !additionalTag.isBeepedAlready &&
                additionalTag.visualCheckStatusID === 1
              ) {
                this.beep("match");
                additionalTag.isBeepedAlready = true;
              }
              stateClone.scanTags.push({ ...additionalTag });
            }
          }
          this.setState(stateClone);
          //const t = stateClone.scanTags.find(t=>t.rfidCheckStatusID === 0);
          //this.setState(stateClone);
        });
    }

    this.setState(stateClone);
  }

  handleScan(data) {
    const stateClone = { ...this.state };
    data = data.trim().toUpperCase();

    console.log(data);

    if (!this.state.receivedBoxID) {
      this.displayAlert(
        "Please first enter received box to scan then continue.",
        "error",
        "",
        0
      );
      //alert("Please first enter received box to scan then continue.");
      return;
    }

    if (stateClone.receivedBox.boxTags.length === 0) {
      this.displayAlert(
        "Please click scan button to get box tags then continue scan tags.",
        "error",
        "",
        0
      );

      /*    alert(
        "Please click scan button to get box tags then continue scan tags."
      ); */
      return;
    }

    console.log(stateClone.receivedBox.boxTags);

    const t = stateClone.receivedBox.boxTags.find(t => t.tagNumber === data);

    console.log(t);

    if (stateClone.receivedBox.boxTags.find(t => t.tagNumber === data)) {
      //alert("found");

      if (!stateClone.scanTags.find(t => t.tagNumber === data)) {
        const tag = stateClone.receivedBox.boxTags.find(
          t => t.tagNumber === data
        );

        tag.visualCheckStatusID = 1;
        //stateClone.scanTags.push({ ...tag });
        stateClone.scanTags.splice(0, 0, { ...tag });
      } else {
        // alert("else");
        const tag = stateClone.scanTags.find(t => t.tagNumber === data);
        console.log(tag);
        tag.visualCheckStatusID = 1;

        /*  const listAfterTagDel = stateClone.scanTags.filter(
          t => t.tagNumber !== tag.tagNumber
        ); */

        if (!tag.isBeepedAlready && tag.rfidCheckStatusID === 1) {
          this.beep("match");
          tag.isBeepedAlready = true;
        }

        /*     listAfterTagDel.splice(0, 0, { ...tag });
        stateClone.scanTags = [...listAfterTagDel]; */
      }
    } else {
      const url = `${
        process.env.REACT_APP_API_URL
      }tag/list?pageSize=${1}&pageNumber=${1}&tagNumber=${data}`;

      this.props
        .getTagsAction(url)
        .then(response => {
          console.log(response);
          const tag = response[0];
          if (tag) {
            this.displayAlert(
              `This tag belongs to ${tag.receivedBoxID} box. Please remove this tag from ${this.state.receivedBoxID} box and move it to box ${tag.receivedBoxID}`,
              "error",
              "",
              0
            );

            /*   alert(
              `This tag belongs to ${tag.receivedBoxID} box. Please remove this tag from ${this.state.receivedBoxID} box and move it to box ${tag.receivedBoxID}`
            ); */
          }
        })
        .catch(error => {
          console.log("Promise emits error");
          let additionalTag = stateClone.additionalTags.find(
            t => t.tagNumber === data
          );

          if (!additionalTag) {
            additionalTag = this.createTag(data);
            additionalTag.visualCheckStatusID = 1;
            stateClone.additionalTags.push({ ...additionalTag });
          } else {
            if (
              !stateClone.scanTags.find(t => t.tagNumber === data) &&
              additionalTag.visualCheckStatusID === 0
            ) {
              additionalTag.visualCheckStatusID = 1;

              if (
                !additionalTag.isBeepedAlready &&
                additionalTag.rfidCheckStatusID === 1
              ) {
                this.beep("match");
                additionalTag.isBeepedAlready = true;
              }
              stateClone.scanTags.push({ ...additionalTag });
            }
          }
          this.setState(stateClone);
        });
    }

    this.setState(stateClone);
  }

  handleError(err) {
    console.error(err);
  }

  handleClick({ target }) {
    const cloneState = { ...this.state };
    switch (target.action) {
      case "back":
        this.props.history.goBack();
        break;
        break;

      case "save":
      case "saveclose":
        if (this.state.scanTags.length === 0) {
          this.displayAlert(
            "Please scan the tags then proceed",
            "error",
            "",
            0
          );
          //alert("Please scan the tags then proceed");
          return;
        }

        const missingTagScan = this.state.receivedBox.boxTags.filter(
          tag => !this.state.scanTags.find(e => e.tagNumber === tag.tagNumber)
        );

        if (missingTagScan.length > 0) {
          this.displayAlert(
            `Warning: Total imported tags in box are =${this.state.receivedBox.boxTags.length} but user scans = ${this.state.scanTags.length}.`,
            "error",
            "",
            0
          );

          /*     alert(
            `Warning: Total imported tags in box are =${this.state.receivedBox.boxTags.length} but user scans = ${this.state.scanTags.length}.`
          ); */
        }
        cloneState.dialogOpen = true;
        cloneState.saveAction = target.action;
        this.setState(cloneState);
        //this.savePrintScanTags();
        break;

      case "saveclose":
        //alert("Please implement saveclose");
        this.displayAlert("Please implement saveclose", "error", "", 0);
        break;

      case "toggleContextMenu":
        const tag = cloneState.scanTags.find(
          t => t.tagNumber === target.tagNumber
        );
        tag.openContextMenu = Boolean(target.element);
        tag.anchorEl = target.element;
        this.setState(cloneState);
        break;

      default:
        if (!this.state.receivedBoxID) {
          this.displayAlert(
            "Please enter received box id to fetch relevant tags.",
            "error",
            "",
            0
          );
          //alert("Please enter received box id to fetch relevant tags.");
          return;
        }

        const url = `${
          process.env.REACT_APP_API_URL
        }received-box/list?pageSize=${250}&pageNumber=${1}&receivedBoxID=${
          cloneState.receivedBoxID
        }`;

        this.props
          .getReceivedBoxAction(url)
          .then(response => {
            console.log(response);
            cloneState.receivedBox = response.Tags;
            cloneState.displayTable = true;
            this.setState(cloneState);
          })
          .catch(error => {
            alert(error);
          });

        break;
    }
  }

  handleChange({ target }) {
    const cloneState = { ...this.state };
    const scanTag = cloneState.scanTags.find(
      tag => tag.tagNumber === target.tagNumber
    );

    switch (target.name) {
      case "txtReceivedBox":
        cloneState.receivedBoxID = target.value;
        console.log(cloneState.receivedBoxID);
        break;
      case "checkBoxVisualCheck":
        scanTag.visualCheckStatusID = scanTag.visualCheckStatusID === 1 ? 0 : 1;
        break;
      case "ddlVisualCheck":
        scanTag.visualCheckStatusID = parseInt(target.value);
        break;
      case "ddlRFIDReasons":
        scanTag.rfidCheckStatusID = parseInt(target.value);
        break;
      case "btnEdit":
        scanTag.inEditMode = true;
        scanTag.rfidCheckStatusID = 2;
        scanTag.appMarkMismatch = false;
        break;
      case "checkBoxRFID":
        scanTag.rfidCheckStatusID = scanTag.rfidCheckStatusID === 1 ? 0 : 1;
        scanTag.inEditMode = false;
        break;
    }

    this.setState(cloneState);
  }

  saveTags() {
    const cloneState = { ...this.state };

    cloneState.scanTags.forEach(
      t =>
        (t.statusID =
          t.visualCheckStatusID === 1 && t.rfidCheckStatusID === 1 ? 1 : 2)
    );

    // filter out  missing tags.
    const missingTags = [
      ...cloneState.receivedBox.boxTags.filter(
        t => !cloneState.scanTags.find(e => e.tagNumber === t.tagNumber)
      )
    ];

    //mark the status of missing tags to missing tag (4)
    missingTags.forEach(
      t => (
        (t.statusID = 4), (t.visualCheckStatusID = 0), (t.rfidCheckStatusID = 0)
      )
    );

    // set the status of received box based on tags status.
    if (
      cloneState.scanTags.filter(t => t.statusID === 1).length ===
      cloneState.receivedBox.boxTags.length
    ) {
      this.state.receivedBoxStatus = 1; // set box status to verified ok
    } else if (missingTags.length > 0) {
      this.state.receivedBoxStatus = 2;
    } else if (cloneState.scanTags.filter(t => !t.isImported) > 0) {
      this.state.receivedBoxStatus = 3;
    }

    // now push missing tags from box tags list to scan tags list
    //missingTags.forEach((t) => cloneState.scanTags.push({ ...t }));

    const transactionData = [...cloneState.scanTags];

    missingTags.forEach(t => transactionData.push({ ...t }));

    console.log(cloneState.scanTags);
    console.log(missingTags);
    console.log(transactionData);
    console.log(this.state.receivedBoxStatus);

    //create ReceivedBox object and its corresponding tags

    const payload = {
      receivedBoxID: this.state.receivedBoxID,
      receivedBoxStatus: this.state.receivedBoxStatus,
      updateUserID: -1,
      scanTags: [...transactionData]
    };

    console.log(payload);

    cloneState.dialogOpen = false;

    const url = `${process.env.REACT_APP_API_URL}received-box/update-scan-tags`;

    this.props
      .updateScanBoxTagsAction(url, payload)
      .then(response => {
        cloneState.receivedBox = { ...response.data };
        cloneState.dialogOpen = false;
        if (this.state.saveAction === "saveclose") {
          this.props.history.push("/tag");
        }
        this.setState(cloneState);
      })
      .catch(error => {
        console.log(error);
        cloneState.dialogOpen = false;
        this.setState(cloneState);
      });

    // now pick transaction data and send it to redux thunk to update the tags status and box status.
    //transactionData send this to server and update
  }

  getReceivedBoxTags(receivedBoxID) {
    if (!this.state.receivedBoxID) {
      this.displayAlert("Please enter received box id", "error", "", 0);
      //alert("Please enter received box id");
      return;
    }

    const url = `${
      process.env.REACT_APP_API_URL
    }tag/list?pageSize=${250}&pageNumber=${1}&receivedBoxID=${receivedBoxID}`;

    this.props.getTagsAction.url = url;
    return this.props.getTagsAction;
  }

  createTag(tagNumber) {
    const serialNumber = parseInt(tagNumber.slice(8, 14), 16);
    const additionalTag = { ...this.state.receivedBox.boxTags[0] };
    additionalTag.tagNumber = tagNumber;
    additionalTag.serialNumber = serialNumber;
    additionalTag.isImported = false;
    //missingTag.visualCheckStatusID = 0;
    //missingTag.rfidCheckStatusID = 0;
    additionalTag.tagID = 0;
    additionalTag.receivedBoxID = this.state.receivedBoxID;
    return additionalTag;
  }

  handleDialogClose(arg, reason) {
    if (reason === "clickaway") {
      return;
    }
    const cloneState = { ...this.state };

    switch (arg.target.elementType) {
      case "alert":
        cloneState.alertOpen = false;
        break;
      case "dialog":
        if (arg.target.action === "continue") {
          this.saveTags();
        } else {
          cloneState.dialogOpen = false;
        }
        break;
      case "contextmenu":
        break;
    }
    this.setState(cloneState);
  }

  displayAlert(content, type, title, duration) {
    /*    const cloneState = { ...this.state };
    cloneState.alertOpen = true;
    cloneState.content = content;
    cloneState.alertType = type;
    cloneState.alertTitle = title;
    cloneState.alertDuration = duration;
    this.setState(cloneState); */

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

  renderDialog() {
    const cloneState = { ...this.state };

    cloneState.scanTags.forEach(
      t =>
        (t.statusID =
          t.visualCheckStatusID === 1 && t.rfidCheckStatusID === 1 ? 1 : 2)
    );

    // filter out  missing tags.
    const missingTags = {
      ...cloneState.receivedBox.boxTags.filter(
        t => !cloneState.scanTags.find(e => e.tagNumber === t.tagNumber)
      )
    };

    // display stats
    const defectiveTagsCount = cloneState.scanTags.filter(t => t.statusID === 2)
      .length;

    const message = `Received box includes ${
      cloneState.receivedBox.boxTags.length
    } tags\n
    ${
      cloneState.scanTags.filter(t => t.statusID === 1).length
    } Tags were verified and scanned\n
    ${
      defectiveTagsCount > 0
        ? `${defectiveTagsCount} Tags were defective and have been moved to defective location (Please remove them from the box)\n`
        : "No tags were defected"
    }  
    ${missingTags.length} Tags were missing from imported tags`;

    return (
      <div className="modelPopup" style={{ paddingLeft: 30 }}>
        <ul>
          <li>
            <p style={{ fontSize: 16, fontWeight: 400 }}>
              Received box includes {cloneState.receivedBox.boxTags.length} tags
            </p>
          </li>
          <li>
            <p style={{ fontSize: 16, fontWeight: 400 }}>
              {cloneState.scanTags.filter(t => t.statusID === 1).length}{" "}
              {"Tags were  verified and scanned"}
            </p>
          </li>
          <li>
            {defectiveTagsCount > 0 ? (
              <p style={{ fontSize: 16, fontWeight: 400 }}>
                {defectiveTagsCount}
                {
                  " Tags were defective and have been moved to defective location (Please remove them from the box)"
                }
              </p>
            ) : (
              <p style={{ fontSize: 16, fontWeight: 400 }}>
                No tags were defected
              </p>
            )}
          </li>

          {cloneState.scanTags.map(t => {
            if (t.visualCheckStatusID === 0 && t.rfidCheckStatusID === 0) {
              return (
                <li>
                  <p
                    style={{ fontSize: 16, fontWeight: 400 }}
                  >{`Tag= ${t.serialNumber} is not verified visually and by reader`}</p>
                </li>
              );
            } else if (t.visualCheckStatusID === 0) {
              return (
                <li>
                  <p
                    style={{ fontSize: 16, fontWeight: 400 }}
                  >{`Tag= ${t.serialNumber} is not verified visually`}</p>
                </li>
              );
            } else if (t.rfidCheckStatusID === 0) {
              return (
                <li>
                  <p
                    style={{ fontSize: 16, fontWeight: 400 }}
                  >{`Tag= ${t.serialNumber} is not verified by reader`}</p>
                </li>
              );
            }
          })}
        </ul>
      </div>
    );
  }

  handleMenuClose(arg) {
    let { element, action, tagNumber } = arg.target;
    const stateClone = { ...this.state };
    action = !action ? "Click Away" : action;

    console.log(arg);
    console.log(action);

    switch (action) {
      case "Delete":
        console.log("event");
        const newScanList = stateClone.scanTags.filter(
          t => t.tagNumber !== tagNumber
        );
        stateClone.scanTags = [...newScanList];

        console.log(newScanList.length);
        console.log(newScanList);

        if (newScanList.length > 0) {
          stateClone.currentTag =
            stateClone.scanTags[stateClone.scanTags.length - 1].tagNumber; // bring last tag as current
        } else {
          stateClone.currentTag = null;
        }

        if (stateClone.wrongTagsLookup.find(t => t.correctTag === tagNumber)) {
          const newWrongTagList = stateClone.wrongTagsLookup.filter(
            t => t.correctTag === tagNumber
          );

          stateClone.wrongTagsLookup = [...newWrongTagList];
        }

        /*    if (
          target.tagNumber === cloneState.currentTag &&
          newScanList.length > 0
        ) {
        } */
        //cloneState.currentTag = null;

        break;
      case "Edit Serial Number":
      case "Edit PIN":
      case "Click Away":
        const tag = stateClone.scanTags.find(t => t.tagNumber === tagNumber);
        tag.anchorEl = null;
        tag.openContextMenu = Boolean(tag.anchorEl);

        if (action === "Click Away") {
          tag.openDialog = false;
        } else {
          if (action === "Edit Serial Number") {
            tag.contextMenuSelectedOption = "Serial";
            tag.dialogTitle = "Edit Serial Number";
          } else {
            tag.contextMenuSelectedOption = "PIN";
            tag.dialogTitle = "Edit Tag PIN";
          }
          tag.openDialog = true;
        }
        break;
    }

    this.setState(stateClone);
  }

  render() {
    return (
      <React.Fragment>
        <BarcodeReader
          onError={this.handleError.bind(this)}
          onScan={this.handleScan.bind(this)}
        />
        {this.state.dialogOpen && (
          <DialogComponent
            title="SUMMARY"
            open={this.state.dialogOpen}
            content={this.renderDialog()}
            actions={[
              { action: "continue", elementType: "dialog", text: "Save Tags" },
              {
                action: "cancel",
                elementType: "dialog",
                text: "Continue Tag Verification"
              }
            ]}
            onDialogClose={this.handleDialogClose.bind(this)}
          ></DialogComponent>
        )}
        {/*  <Snackbar
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
          </Alert> */}

        {/*    <Alert
            onClose={this.handleAlertClose.bind(this)}
            severity="error"
            variant="outlined"
            sx={{ width: "100%" }}
          ></Alert> */}
        {/* </Snackbar> */}
        {/*  <DialogComponent></DialogComponent> */}

        <div className="actions">
          <Actions
            isFormValid={true}
            onLinkClick={this.handleClick.bind(this)}
          ></Actions>
        </div>
        {this.state.showAlert && this.state.showAlert()}
        <div className="content-area-main">
          <div className="content-title">
            <h4>
              Scan Tags
              {!this.props.navigateFrom && ` | ${this.state.receivedBoxID}`}
            </h4>
            {/* <div className="reader-tag-read blink_me">{"Test"}</div> */}
          </div>
          <div className="row top-buffer" style={{ marginLeft: 3 }}></div>

          <div className="row">
            <div className="col-md-4">
              {" "}
              {(!this.state.receivedBoxID ||
                this.props.navigateFrom === "leftMenu") && (
                <FieldSet
                  inputValue={this.state.receivedBoxID}
                  onTextChange={this.handleChange.bind(this)}
                  onButtonClick={this.handleClick.bind(this)}
                  form="scanTag"
                ></FieldSet>
              )}
            </div>
          </div>

          <div className="row top-buffer">
            <div className="list-container">
              {this.state.displayTable && (
                <Table
                  columns={this.columns}
                  data={this.state.scanTags}
                  searchVisible={false}
                ></Table>
              )}
            </div>
          </div>
        </div>
      </React.Fragment>
    );
  }
}

ScanTags.propTypes = {
  receivedBox: PropTypes.object.isRequired,
  searchCount: PropTypes.number.isRequired,
  totalCount: PropTypes.number.isRequired
};

const mapStateToProps = state => {
  return {
    receivedBox: state.receivedBox.receivedBox
  };
};

const mapDispatchToProps = dispatch => ({
  getReceivedBoxAction: url => dispatch(getReceivedBox(url)),
  updateScanBoxTagsAction: (url, payload) =>
    dispatch(updateScanBoxTags(url, payload)),
  getTagsAction: url => dispatch(getTags(url))
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(ScanTags);
