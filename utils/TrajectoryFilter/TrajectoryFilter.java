
import com.vividsolutions.jts.geom.*;
import com.vividsolutions.jts.geom.Polygon;
import org.apache.commons.csv.CSVFormat;
import org.apache.commons.csv.CSVParser;
import org.apache.commons.csv.CSVPrinter;
import org.apache.commons.csv.CSVRecord;

import java.io.*;
import java.util.*;

public class TrajectoryFilter {

    static String inputFile = "floor-0_reduced.csv";
    static String outputFile = "out.csv";
    static Object[] outputFileHeader = {"timeInSec", "pedID", "posX","posY"};

    static GeometryFactory geometryFactory = new GeometryFactory();
    static List<Polygon> inclusionAreas = new ArrayList<>();
    static List<Polygon> exclusionAreas = new ArrayList<>();
    static FileWriter fw = null;


    private static CSVParser getInputFileParser() {
        Reader in = null;
        CSVParser parser = null;
        try {
            in = new FileReader(inputFile);
            parser = new CSVParser(in, CSVFormat.DEFAULT.withHeader());
        } catch (IOException e) { e.printStackTrace(); }
        return parser;
    }

    private static CSVPrinter getOutputFilePrinter() {
        CSVFormat format = CSVFormat.DEFAULT.withRecordSeparator("\n");
        CSVPrinter printer = null;
        try {
            fw = new FileWriter(outputFile);
            printer = new CSVPrinter(fw, format);
            printer.printRecord(outputFileHeader);
        } catch (IOException e) { e.printStackTrace(); }
        return printer;
    }

    private static void defineInclusionAreas () {
        Coordinate[] coords = new Coordinate[5];
        coords[0] = new Coordinate(300, 0);
        coords[1] = new Coordinate(370, 280);
        coords[2] = new Coordinate(0, 280);
        coords[3] = new Coordinate(0, 0);
        coords[4] = new Coordinate(300, 0); // last one must equal the first one -> closed polygon
        inclusionAreas.add(geometryFactory.createPolygon(coords));
        // ...
    }

    private static void defineExclusionAreas() {
        // ...
    }


    public static void main(String[] args) throws IOException {
        defineInclusionAreas();
        defineExclusionAreas();
        Map<Integer, Ped> peds = new HashMap<>();

        // 1. COLLECT PEDS and get coverage-rectangle of each

        for(CSVRecord record : getInputFileParser()) {
            int id = Integer.parseInt(record.get("pedID"));
            double x = Double.parseDouble(record.get("posX"));
            double y = Double.parseDouble(record.get("posY"));
            if(peds.containsKey(id))
                peds.get(id).handle(x, y);
            else
                peds.put(id, new Ped(x, y));
        }

        // 2. PRINT ONLY PEDS that fulfill certain criteria

        //Set<Integer> blocklist = new HashSet<Integer>();
        CSVPrinter printer = getOutputFilePrinter();

        for(CSVRecord record : getInputFileParser()) {
            int id = Integer.parseInt(record.get("pedID"));
            //double time = Double.parseDouble(record.get("timeInSec"));
            //if(time > 30 && time < 90) {
            if(peds.get(id).keepMe()) {
                List newRecord = new ArrayList();
                newRecord.add(record.get("timeInSec"));
                newRecord.add(record.get("pedID"));
                newRecord.add(record.get("posX"));
                newRecord.add(record.get("posY"));
                printer.printRecord(newRecord);
                //System.out.println("added entry for ped " + id);
            }
        }

        fw.flush();
        fw.close();
        printer.close();
    }
}

class Ped {
    public double minX;
    public double minY;
    public double maxX;
    public double maxY;

    public Ped(double x, double y) {
        this.minX = x;
        this.minY = y;
        this.maxX = x;
        this.maxY = y;
    }

    public void handle(double x, double y) {
        if(x < minX)
            minX = x;
        if(x > maxX)
            maxX = x;
        if(y < minY)
            minY = y;
        if(y > maxY)
            maxY = y;
    }

    public boolean didntMove() {
        return minX == maxX && minY == maxY;
    }

    private Polygon getPoly() {
        Coordinate[] coords = new Coordinate[5];
        coords[0] = new Coordinate(minX, minY);
        coords[1] = new Coordinate(minX, maxY);
        coords[2] = new Coordinate(maxX, maxY);
        coords[3] = new Coordinate(maxX, minY);
        coords[4] = new Coordinate(minX, minY);
        Polygon poly = Main.geometryFactory.createPolygon(coords); // a rectangle spanning from the lower left to the upper right corner of the total area i am walking on: coverage-rectangle
        return poly;
    }

    public boolean keepMe() {
        for(Polygon inclusionPoly : Main.inclusionAreas)
            if(!getPoly().intersects(inclusionPoly)) // if my coverage-rectangle doesn't overlap all of the inclusion areas, i am out
                return false;
        for(Polygon exclusionPoly : Main.exclusionAreas) // if my coverage-rectangle overlaps with one of the exclusion areas, i am out
            if(getPoly().intersects(exclusionPoly))
                return false;
        return true;
    }

    public String toString() {
        return minX + " / " + minY + ", " + maxX + " / " + maxY;
    }

}
