import React, { useState, useEffect, useRef } from "react";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import {
  getIssuedBoxes,
  downloadSerialList,
  printLabel,
  selectIssuedBox,
  updateIssuedBoxesStatus
} from "../../../actions/issuedBoxActions";
import config from "../../../config.json";
import Table from "../../common/ui-controls/Table";
import IssuedBoxActions from "../../common/reuseable/IssuedBoxActions";
import ImportScanActions from "../../common/reuseable/ImportScanActions";
import Pagination from "../../common/reuseable/Pagination";
import { Link } from "react-router-dom";
import { renderAlert } from "../../common/ui-controls/withAlert";

const IssuedBoxes = props => {
  const pageNumber = useRef(1);
  const filterSetting = useRef({});
  const [state, setState] = useState({
    issuedBoxList: [{}],
    readOnly: false,
    showAlert: null
  });
  const [issuedBoxList, setIssuedBoxList] = useState([{}]);
  const [disableComplete, setDisableComplete] = useState(true);
  const pageSize = config.pageConfig.pageSize;
  const receivedBoxID = props.match.params.boxid;
  if (receivedBoxID) filterSetting.current["receivedBoxID"] = receivedBoxID;

  const columns = [
    {
      path: "checkBox",
      title: "Select",
      key: 1,
      content: (column, box) => (
        <input
          type="checkbox"
          checked={box.selected}
          onChange={e => handleCheckBoxChange(box)}
        ></input>
      )
    },

    {
      path: "issuedBoxID",
      title: "Issued Box ID",
      key: 2,
      content: id => {
        const navigateUrl = `/issued-box/summary/${id}`;
        return <Link to={navigateUrl}>{id}</Link>;
      }
    },
    { path: "quantity", title: "Quantity", key: 3 },
    { path: "status", title: "Status", key: 4 }
  ];

  const getURL = () => {
    let baseUrl = `${process.env.REACT_APP_API_URL}issued-box/list?pageSize=${pageSize}&pageNumber=${pageNumber.current}`;
    let props = Object.keys(filterSetting.current);

    if (props.length > 0) {
      for (let prop in filterSetting.current) {
        baseUrl = baseUrl + `&${prop}=${filterSetting.current[prop]}`;
      }
    }
    console.log(baseUrl);
    return baseUrl;
  };

  useEffect(() => {
    const url = getURL();
    props.getIssuedBoxesAction(url).then(response => {
      response.forEach(box => (box.selected = false));

      const stateCopy = { ...state };
      stateCopy.issuedBoxList = [...response];
      setState(stateCopy);
    });
  }, []);

  /* useEffect(() => {
    alert("use effect with dependancy");
    alert(state.showAlert);
  }, [state.showAlert]); */

  const handleChange = async ({ value, element, column, event }) => {
    switch (element) {
      case "pagination":
        pageNumber.current = parseInt(value);
        reFetchData();
        break;
      case "dropdown":
        if (parseInt(value) === 0) delete filterSetting.current["statusID"];
        else filterSetting.current["statusID"] = value;
        pageNumber.current = 1;
        reFetchData();
        break;
      case "tablefilter":
        if (column === "checkBox") {
          updateBoxesCheck(value);
        } else {
          if (value) filterSetting.current[column] = value;
          else delete filterSetting.current[column];
          pageNumber.current = 1;
          reFetchData();
        }
        break;
    }
  };
  const reFetchData = () => {
    const url = getURL();
    // props.getIssuedBoxesAction(url);

    props.getIssuedBoxesAction(url).then(response => {
      response.forEach(box => (box.selected = false));
      // setIssuedBoxList([...response]);
      const stateClone = { ...state };
      stateClone.issuedBoxList.forEach(ib => (ib.selected = false));
      setState(stateClone);
    });
  };

  const handleCheckBoxChange = arg => {
    const stateCopy = { ...state };

    const boxToSelect = stateCopy.issuedBoxList.filter(
      box => box.issuedBoxID === arg.issuedBoxID
    )[0];

    boxToSelect.selected = !boxToSelect.selected;

    props.selectIssuedBoxAction(boxToSelect).then(response => {
      stateCopy.readOnly = boxToSelect.selected && boxToSelect.quantity === 0;
      setState(stateCopy);
    });
  };

  const updateBoxesCheck = checked => {
    const stateCopy = { ...state };
    stateCopy.issuedBoxList.forEach(b => (b.selected = checked));

    stateCopy.readOnly =
      stateCopy.issuedBoxList.filter(i => i.selected && i.quantity === 0)
        .length > 0;

    console.log(stateCopy.issuedBoxList);
    props.selectIssuedBoxAction(null, true, checked);
    setState(stateCopy);
  };

  const handleClick = arg => {
    const stateCopy = { ...state };

    console.log(arg);
    switch (arg) {
      case "back":
        props.history.goBack();
        break;

      case "serialList":
        if (
          stateCopy.issuedBoxList.filter(b => b.selected).length === 0 ||
          stateCopy.issuedBoxList.filter(b => b.selected && b.quantity === 0)
            .length > 0
        ) {
          displayAlert(
            "Please select issued box or selected box have 0 quantity.",
            "error"
          );
          //alert("Please select issued box or selected box have 0 quantity.");
          return;
        }

        let issuedBoxIDList = stateCopy.issuedBoxList
          .filter(b => b.selected)
          .map(b => b.issuedBoxID)
          .join(",");
        console.log(issuedBoxIDList);

        let baseUrl = `${process.env.REACT_APP_API_URL}issued-box/serial-list?issuedBoxIDList=${issuedBoxIDList}`;
        console.log(baseUrl);
        props.downloadSerialListAction(baseUrl).then(response => {});
        break;

      case "printlabel":
        if (stateCopy.issuedBoxList.filter(b => b.selected).length === 0) {
          displayAlert("Please select issued box.", "error");

          return;
        }

        /*  alert(
          "Label will print only for boxes having " +
            config.boxCount +
            " verified tags"
        );

        stateCopy.showAlert = renderAlert.bind(
          null,
          "error",
          "Label will print only for boxes having " +
            config.boxCount +
            " verified tags",
          true,
          e => {
            const stateCopy = { ...state };
            stateCopy.showAlert = null;
            setState(stateCopy);
          }
        );

        setState(stateCopy); */

        const boxesForPrintLabel = stateCopy.issuedBoxList.filter(
          b => b.selected && b.quantity === config.boxCount && b.statusID >= 2
        );

        if (boxesForPrintLabel.length === 0) {
          displayAlert(
            "No issued box found to have" +
              config.boxCount +
              " tags or having completed status",
            "error",
            "",
            0
          ).then(r => {});
          return;
        }

        console.log(stateCopy.issuedBoxList);
        console.log(boxesForPrintLabel);

        props
          .printLabelAction(
            `${process.env.REACT_APP_API_URL}issued-box/print-label`,
            boxesForPrintLabel
          )
          .then(res => {
            reFetchData();
          });

        break;

      case "sendToPress":
        if (stateCopy.issuedBoxList.filter(b => b.selected).length === 0) {
          displayAlert("Please select issued box", "error");

          /*  stateCopy.showAlert = renderAlert.bind(
            null,
            "error",
            "Please select issued box",
            true,
            e => {
              const stateCopy = { ...state };
              stateCopy.showAlert = null;
              setState(stateCopy);
            }
          );

          setState(stateCopy); */
          //alert("Please select issued box");
          return;
        }

        if (props.issuedBoxes.filter(b => b.statusID === 3).length === 0) {
          displayAlert(
            "Box with status labelled can only be send to press",
            "error"
          );

          /* stateCopy.showAlert = renderAlert.bind(
            null,
            "error",
            "Box with status labelled can only be send to press",
            true,
            e => {
              const stateCopy = { ...state };
              stateCopy.showAlert = null;
              setState(stateCopy);
            }
          );

          setState(stateCopy); */

          //alert("Box with status labelled can only be send to press");
          return;
        }
        props.history.push("/issued-box/send-to-press");
        break;

      case "boxToDistributor":
        if (stateCopy.issuedBoxList.filter(b => b.selected).length === 0) {
          displayAlert("Please select issued box", "error");

          /* stateCopy.showAlert = renderAlert.bind(
            null,
            "error",
            "Please select issued box",
            true,
            e => {
              const stateCopy = { ...state };
              stateCopy.showAlert = null;
              setState(stateCopy);
            }
          );

          setState(stateCopy);
 */
          //alert("Please select issued box");
          return;
        }

        if (props.issuedBoxes.filter(b => b.statusID === 5).length === 0) {
          displayAlert(
            "Only boxes received from press can be issue to distributor",
            "error"
          );

          /* stateCopy.showAlert = renderAlert.bind(
            null,
            "error",
            "Only boxes received from press can be issue to distributor",
            true,
            e => {
              const stateCopy = { ...state };
              stateCopy.showAlert = null;
              setState(stateCopy);
            }
          );

          setState(stateCopy); */

          //alert("Only boxes received from press can be issue to distributor");
          return;
        }
        props.history.push("/issued-box/issue-box-to-distributor");
        break;

      case "completebox":
        if (stateCopy.issuedBoxList.filter(b => b.selected).length === 0) {
          displayAlert("Please select issued box", "error");

          /*  stateCopy.showAlert = renderAlert.bind(
            null,
            "error",
            "Please select issued box",
            true,
            e => {
              const stateCopy = { ...state };
              stateCopy.showAlert = null;
              setState(stateCopy);
            }
          );

          setState(stateCopy); */
          return;
        }
        console.log(stateCopy.issuedBoxList);

        const boxesToComplete = stateCopy.issuedBoxList.filter(b => b.selected);

        console.log(boxesToComplete);

        boxesToComplete.forEach(b => (b.statusID = 2));

        console.log(boxesToComplete);

        updateIssuedBoxStatus(boxesToComplete).then(r => {
          reFetchData();
        });

        break;
    }
  };

  const updateIssuedBoxStatus = boxList => {
    return new Promise((resolve, reject) => {
      const url = `${process.env.REACT_APP_API_URL}issued-box/update-boxes-status`;

      props.updateIssuedBoxesStatusAction(url, boxList).then(r => {
        resolve(r);
      });
    });
  };

  const displayAlert = (content, type) => {
    return new Promise((resolve, reject) => {
      const stateClone = { ...state };
      stateClone.showAlert = renderAlert.bind(null, type, content, true, e => {
        const stateClone = { ...state };
        stateClone.showAlert = null;
        setState(stateClone);
        resolve("fulfilled");
      });

      setState(stateClone);
    });
  };

  return (
    <React.Fragment>
      <div className="actions">
        <IssuedBoxActions
          onLinkClick={handleClick}
          readOnly={state.readOnly}
        ></IssuedBoxActions>
      </div>
      {state.showAlert && state.showAlert()}
      <div className="content-area-main">
        <div className="row top-buffer">
          <div className="col-3 select-container">
            <select
              className="select-search"
              onChange={e =>
                handleChange({
                  value: e.target.value,
                  element: "dropdown"
                })
              }
            >
              <option className="select-search" value={0} selected>
                All Issued Boxes
              </option>
              <option className="select-search" value={1}>
                Assigned
              </option>
              <option className="select-search" value={2}>
                {" "}
                Created
              </option>
              <option className="select-search" value={3}>
                Labelled
              </option>
              <option className="select-search" value={4}>
                Sent
              </option>
              <option className="select-search" value={5}>
                Received
              </option>
              <option className="select-search" value={6}>
                Exported
              </option>
              <option className="select-search" value={7}>
                Issued
              </option>
            </select>
          </div>
        </div>
        <div className="row top-buffer">
          <div className="list-container">
            {
              <Table
                columns={columns}
                data={props.issuedBoxes}
                searchVisible={true}
                onTableFilter={handleChange}
              ></Table>
            }
          </div>
          <div className="row">
            <Pagination
              totalRecords={props.searchCount}
              pageSize={pageSize}
              onPageChange={handleChange}
              selectedPage={pageNumber.current}
            ></Pagination>
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};

PropTypes.Tags = {
  issuedBoxes: PropTypes.array.isRequired,
  totalCount: PropTypes.number.isRequired,
  searchCount: PropTypes.number.isRequired
};

const mapStateToProps = state => {
  return {
    issuedBoxes: state.issuedBox.issuedBoxes,
    totalCount: state.issuedBox.totalCount,
    searchCount: state.issuedBox.searchCount
  };
};

const mapDispatchToProps = dispatch => ({
  getIssuedBoxesAction: url => dispatch(getIssuedBoxes(url)),
  downloadSerialListAction: url => dispatch(downloadSerialList(url)),
  printLabelAction: (url, issuedBoxList) =>
    dispatch(printLabel(url, issuedBoxList)),
  selectIssuedBoxAction: (box, all, checked) =>
    dispatch(selectIssuedBox(box, all, checked)),
  updateIssuedBoxesStatusAction: (url, boxList) =>
    dispatch(updateIssuedBoxesStatus(url, boxList))
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(IssuedBoxes);
